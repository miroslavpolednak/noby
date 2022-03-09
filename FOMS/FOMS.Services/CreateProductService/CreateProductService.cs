using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Abstraction;
using DomainServices.ProductService.Contracts;
using FOMS.Services.CreateProduct;

namespace FOMS.Services;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
public sealed class CreateProductService
{
    public async Task<long> CreateMortgage(long caseId, int partnerId, GetMortgageDataResponse offerData, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateProductService), caseId);

        var request = new CreateMortgageRequest
        {
            CaseId = caseId,
            Mortgage = offerData.ToDomainServiceRequest(partnerId)
        };
        var result = ServiceCallResult.Resolve<ProductIdReqRes>(await _productService.CreateMortgage(request, cancellationToken));

        _logger.EntityCreated(nameof(Product), result.ProductId);

        return result.ProductId;
    }

    private readonly ILogger<CreateProductService> _logger;
    private readonly IProductServiceAbstraction _productService;

    public CreateProductService(ILogger<CreateProductService> logger, IProductServiceAbstraction productService)
    {
        _logger = logger;
        _productService = productService;
    }
}
