namespace DomainServices.HouseholdService.Api.Handlers.Household.DeleteHousehold;

internal record DeleteHouseholdMediatrRequest(int HouseholdId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}