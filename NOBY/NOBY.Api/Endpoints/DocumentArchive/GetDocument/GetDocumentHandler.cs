using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using NOBY.Infrastructure.Security;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentHandler : IRequestHandler<GetDocumentMediatrRequest, GetDocumentResponse>
{
    private readonly IDocumentArchiveServiceClient _client;
    private readonly IHttpContextAccessor _httpContext;

    public GetDocumentHandler(
        IDocumentArchiveServiceClient client,
        IHttpContextAccessor httpContext)
    {
        _client = client;
        _httpContext = httpContext;
    }

    public async Task<GetDocumentResponse> Handle(GetDocumentMediatrRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContext.HttpContext?.User as FomsUser;
        var result = await _client.GetDocument(new()
        {
            DocumentId = request.DocumentId,
            UserLogin = user is null ? "Unknow NOBY user" : user.Id.ToString(),
            WithContent = true
        }, cancellationToken);

        return result;
    }
}
