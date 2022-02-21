using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal sealed class SearchCustomersMediatrRequest : IRequest<SearchCustomersResponse>, IValidatableRequest
{
    public SearchCustomersRequest Request { get; init; }

    public SearchCustomersMediatrRequest(SearchCustomersRequest request)
    {
        this.Request = request;
    }
}