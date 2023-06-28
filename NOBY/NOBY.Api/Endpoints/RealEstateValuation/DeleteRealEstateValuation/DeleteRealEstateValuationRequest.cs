namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteRealEstateValuation;

internal sealed record DeleteRealEstateValuationRequest(long CaseId, int RealEstateValuationId)
    : IRequest
{
}
