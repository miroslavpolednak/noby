﻿using DomainServices.DocumentArchiveService.Api.Mappers;
using DomainServices.DocumentArchiveService.Contracts;
using ExternalServices.Sdf.V1.Clients;
using ExternalServices.Sdf.V1.Model;
using ExternalServicesTcp.V1.Clients;
using ExternalServicesTcp.V1.Model;
using ExternalServicesTcp.V1.Repositories;
using Google.Protobuf;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocument;

public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, GetDocumentResponse>
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
        if (tcpResult is null)
        {
            throw new ArgumentNullException(nameof(tcpResult));
        }

        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

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
        if (cspResponse.Metadata is null)
        {
            throw new ArgumentNullException(nameof(cspResponse.Metadata));
        }

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
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return await _documentServiceRepository.GetDocumentByExternalId(new GetDocumentByExternalIdTcpQuery
        {
            DocumentId = request.DocumentId,
            WithContent = request.WithContent
        },
        cancellationToken);
    }

    private async Task<GetDocumentByExternalIdOutput> LoadFromCspArchive(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

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
