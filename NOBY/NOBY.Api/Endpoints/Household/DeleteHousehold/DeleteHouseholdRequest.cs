namespace NOBY.Api.Endpoints.Household.DeleteHousehold;

internal sealed record DeleteHouseholdRequest(int HouseholdId)
    : IRequest<int>
{
}
