using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal class GetDetailMediatrRequest : IRequest<GetDetailResponse>, IValidatableRequest
{
    public GetDetailRequest Request { get; init; }

    public GetDetailMediatrRequest(GetDetailRequest request)
    {
        this.Request = request;
    }
}