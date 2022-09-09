using CIS.Core.Validation;

namespace DomainServices.CustomerService.Api.Dto;

internal record SearchCustomersMediatrRequest(SearchCustomersRequest Request)
    : IRequest<SearchCustomersResponse>, IValidatableRequest;