using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetIncome;

internal record GetIncomeMediatrRequest(int IncomeId)
    : IRequest<Income>
{
}
