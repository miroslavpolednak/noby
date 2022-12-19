using CIS.Core.Security;
using DomainServices.DocumentArchiveService.Clients;
using __Contract = DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListHandler : IRequestHandler<GetDocumentListRequest, GetDocumentListResponse>
{
    private readonly IDocumentArchiveServiceClient _client;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetDocumentListHandler(
        IDocumentArchiveServiceClient client,
        ICurrentUserAccessor currentUserAccessor)
    {
        _client = client;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<GetDocumentListResponse> Handle(GetDocumentListRequest request, CancellationToken cancellationToken)
    {
        var user = _currentUserAccessor.User;

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
