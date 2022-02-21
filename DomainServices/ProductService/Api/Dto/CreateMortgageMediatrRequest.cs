using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Dto;

internal sealed class CreateMortgageMediatrRequest
    : IRequest<ProductIdReqRes>, CIS.Core.Validation.IValidatableRequest
{
    public CreateMortgageRequest Request { get; init; }

    public CreateMortgageMediatrRequest(CreateMortgageRequest request)
    {
        Request = request;
    }
}