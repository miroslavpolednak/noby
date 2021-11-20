using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal class GetBasicDataByIdentifierMediatrRequest : IRequest<GetBasicDataResponse>, IValidatableRequest
{
    public GetBasicDataByIdentifierRequest Request { get; init; }

    public GetBasicDataByIdentifierMediatrRequest(GetBasicDataByIdentifierRequest request)
    {
        this.Request = request;
    }
}