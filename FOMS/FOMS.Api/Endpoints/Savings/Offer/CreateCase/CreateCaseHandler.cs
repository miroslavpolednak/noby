using CIS.Core.Exceptions;
using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer;

internal class CreateCaseHandler
    : IRequestHandler<CreateCaseRequest, int>
{
    public async Task<int> Handle(CreateCaseRequest request, CancellationToken cancellationToken)
    {
        return 1;
    }

    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;

    public CreateCaseHandler(DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService)
    {
        _productService = productService;
    }
}
