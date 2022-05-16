using DomainServices.CodebookService.Abstraction;
using DomainServices.ProductService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Product = DomainServices.ProductService.Contracts;
using DomainServices.OfferService.Abstraction;
using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;

namespace FOMS.Api.Notifications.Handlers;

internal class CreateProductHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        if (!notification.NewMpCustomerId.HasValue) return;

        // detail SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(notification.SalesArrangementId, cancellationToken));
        
        // get product type
        var productType = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .First(t => t.Id == saInstance.SalesArrangementTypeId)
            .ProductTypeId;
        // product category
        var productCategory = (await _codebookService.ProductTypes(cancellationToken))
            .First(t => t.Id == productType)
            .ProductCategory;

        long productId = await createProduct(notification.CaseId, saInstance.OfferId!.Value, productCategory, notification.NewMpCustomerId.Value, cancellationToken);

        _logger.EntityCreated(nameof(_Product.Product), productId);
    }

    async Task<long> createProduct(long caseId, int offerId, ProductTypeCategory category, int partnerId, CancellationToken cancellationToken) =>
        category switch
        {
            ProductTypeCategory.Mortgage => await createMortgage(caseId, offerId, partnerId, cancellationToken),
            _ => throw new NotImplementedException("CreateProduct for this product category is not implemented")
        };

    private async Task<long> createMortgage(long caseId, int offerId, int partnerId, CancellationToken cancellationToken)
    {
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.OfferService.Contracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(offerId, cancellationToken));

        var request = new _Product.CreateMortgageRequest
        {
            CaseId = caseId,
            Mortgage = offerInstance.ToDomainServiceRequest(partnerId)
        };
        var result = ServiceCallResult.ResolveAndThrowIfError<_Product.ProductIdReqRes>(await _productService.CreateMortgage(request, cancellationToken));
        return result.ProductId;
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly IProductServiceAbstraction _productService;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        IOfferServiceAbstraction offerService,
        ICodebookServiceAbstraction codebookService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        IProductServiceAbstraction productService,
        ILogger<CreateProductHandler> logger)
    {
        _offerService = offerService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _productService = productService;
        _logger = logger;
    }
}
