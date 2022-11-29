namespace DomainServices.HouseholdService.Api.Endpoints.Household.DeleteHousehold;

internal record DeleteHouseholdMediatrRequest(int HouseholdId, bool HardDelete)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}