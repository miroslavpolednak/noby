using CIS.Core.Results;

namespace FOMS.Api.SharedHandlers;

internal sealed class SharedCreateProductHandler
    : IRequestHandler<Requests.SharedCreateProductRequest, long>
{
    public async Task<long> Handle(Requests.SharedCreateProductRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Attempt to create product instance {data}", request);
        long productId = ServiceCallResult.Resolve<long>(await _productService.CreateProductInstance(request.CaseId, request.ProductTypeId));
        _logger.LogDebug("Product instance #{productId} created", productId);

        return productId;
    }

    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private ILogger<SharedCreateProductHandler> _logger;

    public SharedCreateProductHandler(
        ILogger<SharedCreateProductHandler> logger,
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService)
    {
        _logger = logger;
        _productService = productService;
    }
}
