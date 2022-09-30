namespace DomainServices.HouseholdService.Api.Dto;

internal record DeleteHouseholdMediatrRequest(int HouseholdId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}