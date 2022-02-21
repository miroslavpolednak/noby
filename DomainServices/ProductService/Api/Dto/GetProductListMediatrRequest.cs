using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class GetProductListMediatrRequest
    : IRequest<GetProductListResponse>, CIS.Core.Validation.IValidatableRequest
{
    public CaseIdRequest Request { get; init; }

    public GetProductListMediatrRequest(CaseIdRequest request)
    {
        Request = request;
    }
}