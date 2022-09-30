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
        long? mpId = notification.CustomerIdentifiers?.FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp)?.IdentityId;
        if (!mpId.HasValue) return; // nema modre ID, nezajima me

        try
        {
            await _productService.GetMortgage(notification.CaseId, cancellationToken);

            // detail SA
            var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(notification.SalesArrangementId, cancellationToken));
            if (!saInstance.OfferId.HasValue)
                throw new CisValidationException($"SalesArrangement #{notification.SalesArrangementId} is not bound to Offer");

            // detail offer
            var offerInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.OfferService.Contracts.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId.Value, cancellationToken));

            var request = new _Product.CreateMortgageRequest
            {
                CaseId = notification.CaseId,
                Mortgage = offerInstance.ToDomainServiceRequest(mpId.Value)
            };
            var result = ServiceCallResult.ResolveAndThrowIfError<_Product.ProductIdReqRes>(await _productService.CreateMortgage(request, cancellationToken));

            _logger.EntityCreated(nameof(_Product.Product), result.ProductId);
        }
        catch
        {
            _logger.LogInformation($"Product already exist for CaseId #{notification.CaseId}");
        }
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
