using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using NOBY.Infrastructure.Security;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocument;

public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, GetDocumentResponse>
{
    private readonly IDocumentArchiveServiceClient _client;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetDocumentHandler(
        IDocumentArchiveServiceClient client,
        ICurrentUserAccessor currentUserAccessor)
    {
        _client = client;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<GetDocumentResponse> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var user = _currentUserAccessor.User;

        var result = await _client.GetDocument(new()
        {
            DocumentId = request.DocumentId,
            UserLogin = user is null ? "Unknow NOBY user" : user.Id.ToString(),
            WithContent = true
        }, cancellationToken);

        return result;
    }
}
