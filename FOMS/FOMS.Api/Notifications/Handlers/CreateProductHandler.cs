using DomainServices.ProductService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Product = DomainServices.ProductService.Contracts;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Notifications.Handlers;

internal class CreateProductHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        if (!notification.NewMpCustomerId.HasValue) return;

        // detail SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(notification.SalesArrangementId, cancellationToken));
        
        long productId = await createMortgage(notification.CaseId, saInstance.OfferId!.Value,  notification.NewMpCustomerId.Value, cancellationToken);

        _logger.EntityCreated(nameof(_Product.Product), productId);
    }

    private async Task<long> createMortgage(long caseId, int offerId, long partnerId, CancellationToken cancellationToken)
    {
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.OfferService.Contracts.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(offerId, cancellationToken));

        var request = new _Product.CreateMortgageRequest
        {
            CaseId = caseId,
            Mortgage = offerInstance.ToDomainServiceRequest(partnerId)
        };
        var result = ServiceCallResult.ResolveAndThrowIfError<_Product.ProductIdReqRes>(await _productService.CreateMortgage(request, cancellationToken));
        return result.ProductId;
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly IProductServiceAbstraction _productService;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        IOfferServiceAbstraction offerService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        IProductServiceAbstraction productService,
        ILogger<CreateProductHandler> logger)
    {
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _productService = productService;
        _logger = logger;
    }
}
