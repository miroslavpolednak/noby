using CIS.Core.Results;

namespace FOMS.Api.SharedHandlers;

internal sealed class SharedCreateProductInstanceHandler
    : IRequestHandler<Requests.SharedCreateProductInstanceRequest, long>
{
    public async Task<long> Handle(Requests.SharedCreateProductInstanceRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Attempt to create product instance {data}", request);
        long productId = ServiceCallResult.Resolve<long>(await _productService.CreateProductInstance(request.CaseId, request.ProductInstanceType));
        _logger.LogDebug("Product instance #{productId} created", productId);

        return productId;
    }

    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private ILogger<SharedCreateProductInstanceHandler> _logger;

    public SharedCreateProductInstanceHandler(
        ILogger<SharedCreateProductInstanceHandler> logger,
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService)
    {
        _logger = logger;
        _productService = productService;
    }
}
