namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteObligation;

internal record DeleteObligationMediatrRequest(int ObligationId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
