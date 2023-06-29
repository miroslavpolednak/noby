namespace NOBY.Api.Endpoints.RealEstateValuation.GetListRealEstateValuation;

internal sealed record GetListRealEstateValuationRequest(long CaseId)
    : IRequest<List<RealEstateValuationListItem>>
{
}
