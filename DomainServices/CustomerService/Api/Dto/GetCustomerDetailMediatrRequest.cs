using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api.Dto;

internal record GetCustomerDetailMediatrRequest(CustomerDetailRequest Request) 
    : IRequest<CustomerDetailResponse>, IValidatableRequest;