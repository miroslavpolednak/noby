using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetObligation;

internal record GetObligationMediatrRequest(int ObligationId)
    : IRequest<Obligation>
{
}
