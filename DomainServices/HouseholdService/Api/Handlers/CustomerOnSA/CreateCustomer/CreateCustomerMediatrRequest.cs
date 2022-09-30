using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Handlers.CustomerOnSA.CreateCustomer;

internal record CreateCustomerMediatrRequest(CreateCustomerRequest Request)
    : IRequest<CreateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{
}