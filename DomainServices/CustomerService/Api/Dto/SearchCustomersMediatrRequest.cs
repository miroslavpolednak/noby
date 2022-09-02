using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api.Dto;

internal record SearchCustomersMediatrRequest(SearchCustomersRequest Request)
    : IRequest<SearchCustomersResponse>, IValidatableRequest;