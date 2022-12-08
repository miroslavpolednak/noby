using DomainServices.DocumentArchiveService.Api.Mappers;
using DomainServices.DocumentArchiveService.Contracts;
using ExternalServices.Sdf.V1.Clients;
using ExternalServicesTcp.V1.Repositories;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GetDocumentList;

public class GetDocumentListMediatrRequestHandler : IRequestHandler<GetDocumentListMediatrRequest, GetDocumentListResponse>
{
    private readonly ISdfClient _sdfClient;
    private readonly IDocumentServiceRepository _tcpRepository;
    private readonly IDocumentMapper _documentMapper;

    public GetDocumentListMediatrRequestHandler(
        ISdfClient sdfClient,
        IDocumentServiceRepository tcpRepository,
        IDocumentMapper documentMapper)
    {
        _sdfClient = sdfClient;
        _tcpRepository = tcpRepository;
        _documentMapper = documentMapper;
    }

    public async Task<GetDocumentListResponse> Handle(GetDocumentListMediatrRequest request, CancellationToken cancellationToken)
    {
        var sdfResult = await _sdfClient.FindDocuments(_documentMapper.MapSdfFindDocumentQuery(request.Request), cancellationToken);

        var response = new GetDocumentListResponse();

        if (sdfResult.DocInfos.Any())
        {
            response.Metadata
            .AddRange(sdfResult.DocInfos
                       .Select(r => _documentMapper.MapSdfDocumentMetadata(r.Metadata.ToArray())));
        }

        var tcpResult = await _tcpRepository.FindTcpDocument(_documentMapper.MapTcpDocumentQuery(request.Request), cancellationToken);
        response.Metadata.AddRange(tcpResult.Select(_documentMapper.MapTcpDocumentMetadata));
        
        return response;
    }
}
