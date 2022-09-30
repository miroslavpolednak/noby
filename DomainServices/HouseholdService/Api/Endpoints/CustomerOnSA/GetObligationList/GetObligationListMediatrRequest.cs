using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetObligationList;

internal record GetObligationListMediatrRequest(int CustomerOnSAId)
    : IRequest<GetObligationListResponse>
{
}
