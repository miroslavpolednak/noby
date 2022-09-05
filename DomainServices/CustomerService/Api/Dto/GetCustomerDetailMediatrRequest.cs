using CIS.Core.Validation;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api.Dto;

internal record GetCustomerDetailMediatrRequest(Identity Identity) 
    : IRequest<CustomerDetailResponse>, IValidatableRequest;