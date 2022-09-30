namespace DomainServices.HouseholdService.Api.Dto;

internal record DeleteObligationMediatrRequest(int ObligationId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
