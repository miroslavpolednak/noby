namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationList;

internal sealed record GetRealEstateValuationListRequest(long CaseId)
    : IRequest<List<RealEstateValuationSharedRealEstateValuationListItem>>
{
}
