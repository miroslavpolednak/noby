using CIS.Core.Validation;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api.Dto;

internal record GetCustomerListMediatrRequest(IList<Identity> Identities) 
    : IRequest<CustomerListResponse>, IValidatableRequest;