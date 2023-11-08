using DomainServices.DocumentArchiveService.Api.Database;
using DomainServices.DocumentArchiveService.Api.Extensions;
using DomainServices.DocumentArchiveService.Api.Mappers;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Model;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Clients;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Model;
using Google.Protobuf;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocument;

internal sealed class GetDocumentHandler : IRequestHandler<GetDocumentRequest, GetDocumentResponse>
{
    private static int[] _allowedDocStates = new[] { 100, 110, 200 };
    private const string DocumentPrefix = "KBH";
    private readonly ISdfClient _sdfClient;
    private readonly IDocumentServiceRepository _documentServiceRepository;
    private readonly ITcpClient _tcpClient;
    private readonly IDocumentMapper _documentMapper;
    private readonly ILogger<GetDocumentHandler> _logger;
    private readonly DocumentArchiveDbContext _dbContext;

    public GetDocumentHandler(
        ISdfClient sdfClient,
        IDocumentServiceRepository documentServiceRepository,
        ITcpClient tcpClient,
        IDocumentMapper documentMapper,
        ILogger<GetDocumentHandler> logger,
        DocumentArchiveDbContext dbContext)
    {
        _sdfClient = sdfClient;
        _documentServiceRepository = documentServiceRepository;
        _tcpClient = tcpClient;
        _documentMapper = documentMapper;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<GetDocumentResponse> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.DocumentId.StartsWith(DocumentPrefix, StringComparison.InvariantCultureIgnoreCase))
            {
                var cspResponse = await LoadFromCspArchive(request, cancellationToken);
                return MapCspResponse(cspResponse);
            }
            else
            {
                var tcpResult = await LoadFromTcpArchive(request, cancellationToken);
                return await MapTcpResponse(tcpResult, request, cancellationToken);
            }
        }
        catch (Exception exp) when (request.GetLocalCopyAsBackup)
        {
            _logger.LocalDocCopyIfNotExistInEArchive(exp);
            return await TryFindDocumentInQueue(request, cancellationToken);
        }
    }

    private async Task<GetDocumentResponse> TryFindDocumentInQueue(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var docFromQueue = await _dbContext.DocumentInterface
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.DocumentId == request.DocumentId && _allowedDocStates.Contains(d.Status), cancellationToken)
                  ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentWithEArchiveIdNotExistInQueue);

        var response = new GetDocumentResponse
        {
            Metadata = _documentMapper.MapEntityDocumentMetadata(docFromQueue),
            Content = new()
        };

        if (request.WithContent)
        {
            response.Content.BinaryData = ByteString.CopyFrom(docFromQueue.DocumentData);
        }

        _ = new FileExtensionContentTypeProvider().TryGetContentType(docFromQueue.FileName, out var mimeType);
        response.Content.MineType = mimeType ?? "application/octet-stream";
        return response;
    }

    private async Task<GetDocumentResponse> MapTcpResponse(DocumentServiceQueryResult tcpResult, GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var response = new GetDocumentResponse
        {
            Metadata = _documentMapper.MapTcpDocumentMetadata(tcpResult),
            Content = new()
        };

        if (request.WithContent)
        {
            response.Content.BinaryData = ByteString.CopyFrom(await _tcpClient.DownloadFile(tcpResult.Url, cancellationToken));
        }

        response.Content.MineType = tcpResult.MimeType ?? string.Empty;
        return response;
    }

    private GetDocumentResponse MapCspResponse(GetDocumentByExternalIdOutput cspResponse)
    {
        var response = new GetDocumentResponse
        {
            Metadata = _documentMapper.MapSdfDocumentMetadata(cspResponse.Metadata),
            Content = new()
        };

        if (cspResponse.FileContent is not null)
        {
            response.Content.BinaryData = ByteString.CopyFrom(cspResponse.FileContent);
        }
        response.Content.MineType = cspResponse.DmsDocInfo?.MimeType ?? string.Empty;
        return response;
    }

    private async Task<DocumentServiceQueryResult> LoadFromTcpArchive(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        return await _documentServiceRepository.GetDocumentByExternalId(new GetDocumentByExternalIdTcpQuery
        {
            DocumentId = request.DocumentId,
            WithContent = request.WithContent
        },
        cancellationToken);
    }

    private async Task<GetDocumentByExternalIdOutput> LoadFromCspArchive(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        return await _sdfClient
       .GetDocumentByExternalId(new GetDocumentByExternalIdSdfQuery
       {
           DocumentId = request.DocumentId,
           WithContent = request.WithContent,
           UserLogin = request.UserLogin
       },
       cancellationToken);
    }
}
