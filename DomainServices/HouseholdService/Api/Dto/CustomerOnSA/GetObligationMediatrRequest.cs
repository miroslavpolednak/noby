using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record GetObligationMediatrRequest(int ObligationId)
    : IRequest<Obligation>
{
}
