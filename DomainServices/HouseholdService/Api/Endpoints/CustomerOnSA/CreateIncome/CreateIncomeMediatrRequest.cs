using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateIncome;

internal record CreateIncomeMediatrRequest(CreateIncomeRequest Request)
    : IRequest<CreateIncomeResponse>, CIS.Core.Validation.IValidatableRequest
{
}
