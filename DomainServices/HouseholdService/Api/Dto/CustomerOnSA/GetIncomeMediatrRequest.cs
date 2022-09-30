using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record GetIncomeMediatrRequest(int IncomeId)
    : IRequest<Income>
{
}
