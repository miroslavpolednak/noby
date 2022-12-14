using DomainServices.DocumentArchiveService.Clients;
using NOBY.Infrastructure.Security;
using __Contract = DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListHandler : IRequestHandler<GetDocumentListMediatrRequest, GetDocumentListResponse>
{
    private readonly IDocumentArchiveServiceClient _client;
    private readonly IHttpContextAccessor _httpContext;

    public GetDocumentListHandler(
        IDocumentArchiveServiceClient client,
        IHttpContextAccessor httpContext)
    {
        _client = client;
        _httpContext = httpContext;
    }

    public async Task<GetDocumentListResponse> Handle(GetDocumentListMediatrRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContext.HttpContext?.User as FomsUser;

        var result = await _client.GetDocumentList(new()
        {
            CaseId = request.CaseId,
            UserLogin = user is null ? "Unknow NOBY user" : user.Id.ToString(),

        }, cancellationToken);

        return MapResponse(result);
    }

    private static GetDocumentListResponse MapResponse(__Contract.GetDocumentListResponse result)
    {
        return new GetDocumentListResponse
        {
            DocumentsMetadata = result.Metadata.Select(s => new DocumentsMetadata
            {
                DocumentId = s.DocumentId,
                EaCodeMainId = s.EaCodeMainId,
                FileName = s.Filename,
                Description = s.Description,
                CreatedOn = s.CreatedOn
            })
            .ToList()
        };
    }
}
