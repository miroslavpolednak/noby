using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record GetIncomeListMediatrRequest(int CustomerOnSAId)
    : IRequest<GetIncomeListResponse>
{
}
