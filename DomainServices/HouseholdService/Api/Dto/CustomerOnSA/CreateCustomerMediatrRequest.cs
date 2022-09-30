using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record CreateCustomerMediatrRequest(CreateCustomerRequest Request)
    : IRequest<CreateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{
}