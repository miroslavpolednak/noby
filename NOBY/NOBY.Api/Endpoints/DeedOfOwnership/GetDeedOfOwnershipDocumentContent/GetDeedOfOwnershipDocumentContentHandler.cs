using ExternalServices.Crem.V1;

namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipDocumentContent;

internal sealed class GetDeedOfOwnershipDocumentContentHandler
    : IRequestHandler<GetDeedOfOwnershipDocumentContentRequest, GetDeedOfOwnershipDocumentContentResponse>
{
    public async Task<GetDeedOfOwnershipDocumentContentResponse> Handle(GetDeedOfOwnershipDocumentContentRequest request, CancellationToken cancellationToken)
    {
        return null;
    }

    private readonly ICremClient _cremClient;

    public GetDeedOfOwnershipDocumentContentHandler(ICremClient cremClient)
    {
        _cremClient = cremClient;
    }
}
