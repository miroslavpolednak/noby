using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record CreateIncomeMediatrRequest(CreateIncomeRequest Request)
    : IRequest<CreateIncomeResponse>, CIS.Core.Validation.IValidatableRequest
{
}
