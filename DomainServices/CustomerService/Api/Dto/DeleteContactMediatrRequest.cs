using CIS.Core.Validation;
using Google.Protobuf.WellKnownTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal class DeleteContactMediatrRequest : IRequest<Empty>, IValidatableRequest 
{
    public DeleteContactRequest Request { get; init; }

    public DeleteContactMediatrRequest(DeleteContactRequest request)
    {
        this.Request = request;
    }
}