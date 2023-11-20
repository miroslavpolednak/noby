using DomainServices.DocumentArchiveService.Api.Mappers;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocumentList;

internal class GetDocumentListHandler : IRequestHandler<GetDocumentListRequest, GetDocumentListResponse>
{
    private readonly ISdfClient _sdfClient;
    private readonly IDocumentServiceRepository _tcpRepository;
    private readonly IDocumentMapper _documentMapper;

    public GetDocumentListHandler(
        ISdfClient sdfClient,
        IDocumentServiceRepository tcpRepository,
        IDocumentMapper documentMapper)
    {
        _sdfClient = sdfClient;
        _tcpRepository = tcpRepository;
        _documentMapper = documentMapper;
    }

    public async Task<GetDocumentListResponse> Handle(GetDocumentListRequest request, CancellationToken cancellationToken)
    {
        var response = new GetDocumentListResponse();

        if (request.SourceArchive == SourceArchive.Sdf)
        {
            await LoadFromSdf(request, response, cancellationToken);
        }
        else if (request.SourceArchive == SourceArchive.Tcp)
        {
            await LoadFromTcp(request, response, cancellationToken);
        }
        else
        {
            // Merge
            await LoadFromSdf(request, response, cancellationToken);
            await LoadFromTcp(request, response, cancellationToken);
        }

        return response;
    }

    private async Task LoadFromTcp(GetDocumentListRequest request, GetDocumentListResponse response, CancellationToken cancellationToken)
    {
        var tcpResult = await _tcpRepository.FindTcpDocument(_documentMapper.MapTcpDocumentQuery(request), cancellationToken);
        response.Metadata.AddRange(tcpResult.Select(_documentMapper.MapTcpDocumentMetadata));
    }

    private async Task LoadFromSdf(GetDocumentListRequest request, GetDocumentListResponse response, CancellationToken cancellationToken)
    {
        var sdfResult = await _sdfClient.FindDocuments(_documentMapper.MapSdfFindDocumentQuery(request), cancellationToken);
        if (sdfResult.DocInfos.Any())
        {
            response.Metadata
            .AddRange(sdfResult.DocInfos
                       .Select(r => _documentMapper.MapSdfDocumentMetadata(r.Metadata.ToArray(), r.DmsDocInfo)));
        }
    }
}
