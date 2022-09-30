using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Dto;

internal record UpdateCustomerMediatrRequest(UpdateCustomerRequest Request)
    : IRequest<UpdateCustomerResponse>, CIS.Core.Validation.IValidatableRequest
{
}