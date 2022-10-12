using DomainServices.ProductService.Abstraction;
using DomainServices.ProductService.Contracts;
using Contracts = DomainServices.ProductService.Contracts;

namespace FOMS.Api.Endpoints.Product.GetProductObligationList;

internal sealed class GetProductObligationListHandler : IRequestHandler<GetProductObligationListRequest, List<ProductObligation>>
{
    private readonly IProductServiceAbstraction _productService;

    public GetProductObligationListHandler(IProductServiceAbstraction productService)
    {
        _productService = productService;
    }

    public async Task<List<ProductObligation>> Handle(
        GetProductObligationListRequest request,
        CancellationToken cancellationToken)
    {
        var response = ServiceCallResult.ResolveAndThrowIfError<GetProductObligationListResponse>(
            await _productService.GetProductObligationList(
                new Contracts.GetProductObligationListRequest { ProductId =  request.ProductId }, cancellationToken));

        return response.ProductObligations.ToList();
    }
}