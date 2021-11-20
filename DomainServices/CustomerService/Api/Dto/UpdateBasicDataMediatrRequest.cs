using CIS.Core.Validation;
using Google.Protobuf.WellKnownTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal class UpdateBasicDataMediatrRequest : IRequest<Empty>, IValidatableRequest
{
    public UpdateBasicDataRequest Request { get; init; }

    public UpdateBasicDataMediatrRequest(UpdateBasicDataRequest request)
    {
        this.Request = request;
    }
}