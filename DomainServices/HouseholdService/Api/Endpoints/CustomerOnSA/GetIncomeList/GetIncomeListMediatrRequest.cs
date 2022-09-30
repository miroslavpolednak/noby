using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetIncomeList;

internal record GetIncomeListMediatrRequest(int CustomerOnSAId)
    : IRequest<GetIncomeListResponse>
{
}
