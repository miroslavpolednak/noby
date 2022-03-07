namespace FOMS.Api.Endpoints.Household.DeleteHousehold;

internal record DeleteHouseholdRequest(int HouseholdId)
    : IRequest<int>
{
}
