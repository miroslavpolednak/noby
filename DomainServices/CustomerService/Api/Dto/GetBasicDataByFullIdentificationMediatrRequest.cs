using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal class GetBasicDataByFullIdentificationMediatrRequest : IRequest<GetBasicDataResponse>, IValidatableRequest
{
    public GetBasicDataByFullIdentificationRequest Request { get; init; }

    public GetBasicDataByFullIdentificationMediatrRequest(GetBasicDataByFullIdentificationRequest request)
    {
        this.Request = request;
    }
}