using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Dto;

internal class GetListMediatrRequest : IRequest<GetListResponse>, IValidatableRequest
{
    public GetListRequest Request { get; init; }

    public GetListMediatrRequest(GetListRequest request)
    {
        this.Request = request;
    }
}