namespace NOBY.Api.Endpoints.Household.DeleteHousehold;

internal record DeleteHouseholdRequest(int HouseholdId)
    : IRequest<int>
{
}
