namespace DomainServices.HouseholdService.Api.Endpoints.Household.GetHousehold;

internal record GetHouseholdMediatrRequest(int HouseholdId)
    : IRequest<Contracts.Household>
{
}