using DomainServices.ProductService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Cu = DomainServices.CustomerService.Contracts;
using DomainServices.OfferService.Abstraction;
using CIS.Infrastructure.gRPC.CisTypes;
using _Product = DomainServices.ProductService.Contracts;

namespace FOMS.Api.Notifications.Handlers;

internal class CreateProductHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        var mpIdentity = notification.CustomerIdentifiers?.FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Mp);
        long? mpId = mpIdentity?.IdentityId;
        if (!mpId.HasValue)
        {
            _logger.LogInformation($"CreateProductHandler for CaseId #{notification.CaseId} not proceeding / missing MP ID");
            return; // nema modre ID, nezajima me
        }

        try
        {
            // proc to tady bylo?
            await _productService.GetMortgage(notification.CaseId, cancellationToken);
            _logger.LogInformation($"Product already exist for CaseId #{notification.CaseId}");
            return;
        }
        catch { }

        // detail SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(notification.SalesArrangementId, cancellationToken));
        if (!saInstance.OfferId.HasValue)
            throw new CisValidationException($"SalesArrangement #{notification.SalesArrangementId} is not bound to Offer");

        // detail offer
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.OfferService.Contracts.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId.Value, cancellationToken));

        // pokud neexistuje customer v konsDb, tak ho vytvor
        try
        {
            var customerDetail = ServiceCallResult.ResolveAndThrowIfError<_Cu.CustomerDetailResponse>(await _customerService.GetCustomerDetail(notification.CustomerIdentifiers!.First(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb), cancellationToken));

            // zalozit noveho klienta
            var createCustomerRequest = new _Cu.CreateCustomerRequest
            {
                Identity = mpIdentity,
                HardCreate = true,
                NaturalPerson = customerDetail.NaturalPerson
            };
            if (customerDetail.Addresses is not null)
                createCustomerRequest.Addresses.AddRange(customerDetail.Addresses);

            await _customerService.CreateCustomer(createCustomerRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            var x = ex.Message;
        }

        var request = new _Product.CreateMortgageRequest
        {
            CaseId = notification.CaseId,
            Mortgage = offerInstance.ToDomainServiceRequest(mpId.Value)
        };
        var result = ServiceCallResult.ResolveAndThrowIfError<_Product.ProductIdReqRes>(await _productService.CreateMortgage(request, cancellationToken));

        _logger.EntityCreated(nameof(_Product.Product), result.ProductId);
    }

    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly IProductServiceAbstraction _productService;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        IOfferServiceAbstraction offerService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        IProductServiceAbstraction productService,
        ILogger<CreateProductHandler> logger)
    {
        _customerService = customerService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _productService = productService;
        _logger = logger;
    }
}
