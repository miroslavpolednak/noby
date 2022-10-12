using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class GetProductObligationListMediatrRequest
    : IRequest<GetProductObligationListResponse>, CIS.Core.Validation.IValidatableRequest
{
    public GetProductObligationListRequest Request { get; init; }

    public GetProductObligationListMediatrRequest(GetProductObligationListRequest request)
    {
        Request = request;
    }
}