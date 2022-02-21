using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class GetMortgageMediatrRequest
    : IRequest<GetMortgageResponse>, CIS.Core.Validation.IValidatableRequest
{
    public ProductIdReqRes Request { get; init; }

    public GetMortgageMediatrRequest(ProductIdReqRes request)
    {
        Request = request;
    }
}