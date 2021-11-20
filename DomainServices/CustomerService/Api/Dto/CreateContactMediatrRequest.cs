using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal class CreateContactMediatrRequest : IRequest<CreateContactResponse>, IValidatableRequest
{
    public CreateContactRequest Request { get; init; }

    public CreateContactMediatrRequest(CreateContactRequest request)
    {
        this.Request = request;
    }
}