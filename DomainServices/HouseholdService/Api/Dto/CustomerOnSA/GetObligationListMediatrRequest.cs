using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record GetObligationListMediatrRequest(int CustomerOnSAId)
    : IRequest<GetObligationListResponse>
{
}
