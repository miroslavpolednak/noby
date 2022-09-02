using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api.Dto;

internal record GetCustomerListMediatrRequest(CustomerListRequest Request) 
    : IRequest<CustomerListResponse>, IValidatableRequest;