namespace FOMS.Api.Endpoints.Household.GetHousehold;

internal record GetHouseholdRequest(int HouseholdId)
    : IRequest<GetHouseholdResponse>
{
}
