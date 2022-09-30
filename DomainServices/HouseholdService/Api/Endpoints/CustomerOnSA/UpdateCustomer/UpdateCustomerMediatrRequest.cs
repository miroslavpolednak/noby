using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateCustomer;

internal record UpdateCustomerMediatrRequest(UpdateCustomerRequest Request)
    : IRequest<UpdateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{
}