using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal sealed class GetCustomerDetailMediatrRequest : IRequest<CustomerResponse>, IValidatableRequest
{
    public CustomerRequest Request { get; init; }

    public GetCustomerDetailMediatrRequest(CustomerRequest request)
    {
        this.Request = request;
    }
}