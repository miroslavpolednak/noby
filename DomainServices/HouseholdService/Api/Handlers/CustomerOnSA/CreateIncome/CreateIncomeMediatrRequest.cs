using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Handlers.CustomerOnSA.CreateIncome;

internal record CreateIncomeMediatrRequest(CreateIncomeRequest Request)
    : IRequest<CreateIncomeResponse>, CIS.Core.Validation.IValidatableRequest
{
}
