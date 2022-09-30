namespace DomainServices.HouseholdService.Api.Endpoints.Household.DeleteHousehold;

internal record DeleteHouseholdMediatrRequest(int HouseholdId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}