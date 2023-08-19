namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationTypes;

internal sealed record GetRealEstateValuationTypesRequest(long CaseId, int RealEstateValuationId)
    : IRequest<List<int>>
{
}
