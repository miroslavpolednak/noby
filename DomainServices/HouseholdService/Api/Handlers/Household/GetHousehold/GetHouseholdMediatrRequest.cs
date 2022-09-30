namespace DomainServices.HouseholdService.Api.Handlers.Household.GetHousehold;

internal record GetHouseholdMediatrRequest(int HouseholdId)
    : IRequest<Contracts.Household>
{
}