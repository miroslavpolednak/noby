using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateCustomer;

internal record CreateCustomerMediatrRequest(CreateCustomerRequest Request)
    : IRequest<CreateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{
}