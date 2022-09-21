using CIS.Core.Validation;

namespace DomainServices.CustomerService.Api.Dto;

internal record CreateCustomerMediatrRequest(CreateCustomerRequest Request) : 
    IRequest<CreateCustomerResponse>, IValidatableRequest;