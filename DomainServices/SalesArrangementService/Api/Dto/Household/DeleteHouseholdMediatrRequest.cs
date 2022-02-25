namespace DomainServices.SalesArrangementService.Api.Dto;

internal record DeleteHouseholdMediatrRequest(int HouseholdId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}