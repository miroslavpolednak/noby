using DomainServices.DocumentArchiveService.Api.Mappers;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Model;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Clients;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Model;
using Google.Protobuf;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocument;

internal class GetDocumentHandler : IRequestHandler<GetDocumentRequest, GetDocumentResponse>
{
    private const string DocumentPrefix = "KBH";
    private readonly ISdfClient _sdfClient;
    private readonly IDocumentServiceRepository _documentServiceRepository;
    private readonly ITcpClient _tcpClient;
    private readonly IDocumentMapper _documentMapper;

    public GetDocumentHandler(
        ISdfClient sdfClient,
        IDocumentServiceRepository documentServiceRepository,
        ITcpClient tcpClient,
        IDocumentMapper documentMapper)
    {
        _sdfClient = sdfClient;
        _documentServiceRepository = documentServiceRepository;
        _tcpClient = tcpClient;
        _documentMapper = documentMapper;
    }

    public async Task<GetDocumentResponse> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        if (request.DocumentId.StartsWith(DocumentPrefix))
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

    private async Task<GetDocumentResponse> MapTcpResponse(DocumentServiceQueryResult tcpResult, GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var response = new GetDocumentResponse
        {
            Metadata = _documentMapper.MapTcpDocumentMetadata(tcpResult),
            Content = new Contracts.FileInfo()
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
            Content = new Contracts.FileInfo()
        };

        if (cspResponse.FileContent is not null)
        {
            response.Content.BinaryData = ByteString.CopyFrom(cspResponse.FileContent);
        }
        response.Content.MineType = cspResponse.DmsDocInfo.MimeType;
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
