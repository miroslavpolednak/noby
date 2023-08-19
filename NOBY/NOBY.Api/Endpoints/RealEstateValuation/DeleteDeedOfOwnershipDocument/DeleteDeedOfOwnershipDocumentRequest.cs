namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteDeedOfOwnershipDocument;

internal sealed record DeleteDeedOfOwnershipDocumentRequest(long CaseId, int RealEstateValuationId, int DeedOfOwnershipDocumentId)
    : IRequest
{
}
