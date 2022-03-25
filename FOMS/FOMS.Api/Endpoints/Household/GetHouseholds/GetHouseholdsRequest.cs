namespace FOMS.Api.Endpoints.Household.GetHouseholds;

internal record GetHouseholdsRequest(int SalesArrangementId)
    : IRequest<List<Dto.HouseholdInList>>
{
}