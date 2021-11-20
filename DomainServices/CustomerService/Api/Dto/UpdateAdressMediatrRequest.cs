using CIS.Core.Validation;
using Google.Protobuf.WellKnownTypes;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal class UpdateAdressMediatrRequest : IRequest<Empty>, IValidatableRequest
{
    public UpdateAdressRequest Request { get; init; }

    public UpdateAdressMediatrRequest(UpdateAdressRequest request)
    {
        this.Request = request;
    }
}