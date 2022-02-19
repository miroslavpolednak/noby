using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal sealed class GetCustomerListMediatrRequest : IRequest<CustomerListResponse>, IValidatableRequest
{
    public CustomerListRequest Request { get; init; }

    public GetCustomerListMediatrRequest(CustomerListRequest request)
    {
        this.Request = request;
    }
}