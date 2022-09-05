using CIS.Core.Validation;

namespace DomainServices.CustomerService.Api.Dto;

internal record GetCustomerDetailMediatrRequest(Identity Identity) 
    : IRequest<CustomerDetailResponse>, IValidatableRequest;