using CIS.Core.Validation;

namespace DomainServices.CustomerService.Api.Dto;

internal record GetCustomerListMediatrRequest(IList<Identity> Identities) 
    : IRequest<CustomerListResponse>, IValidatableRequest;