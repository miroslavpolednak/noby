namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipDocumentContent;

internal sealed record GetDeedOfOwnershipDocumentContentRequest(
    int? KatuzId,
    int? DeedOfOwnershipNumber,
    long? DeedOfOwnershipId,
    int? DeedOfOwnershipDocumentId,
    long? CremDeedOfOwnershipDocumentId)
    : IRequest<DeedOfOwnershipGetDeedOfOwnershipDocumentContentResponse>
{
}
