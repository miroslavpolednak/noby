using CIS.Core.Validation;

namespace DomainServices.CustomerService.Api.Dto;

internal record CreateCustomerMediatrRequest : 
    IRequest<CreateCustomerResponse>, IValidatableRequest;