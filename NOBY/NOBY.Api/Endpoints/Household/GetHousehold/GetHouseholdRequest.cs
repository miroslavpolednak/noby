namespace NOBY.Api.Endpoints.Household.GetHousehold;

internal sealed record GetHouseholdRequest(int HouseholdId)
    : IRequest<GetHouseholdResponse>
{
}
