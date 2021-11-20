using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal class CreateMediatrRequest : IRequest<CreateResponse>, IValidatableRequest
{
    public CreateRequest Request { get; init; }

    public CreateMediatrRequest(CreateRequest request)
    {
        this.Request = request;
    }
}