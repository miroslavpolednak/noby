using CIS.Foms.Enums;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.OfferService.Clients;
using CIS.Infrastructure.gRPC.CisTypes;
using _Product = DomainServices.ProductService.Contracts;
using CIS.Infrastructure.CisMediatR.Rollback;
using NOBY.Api.Endpoints.Offer.CreateMortgageCase;

namespace NOBY.Api.Notifications.Handlers;

internal sealed class CreateProductHandler
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
            // je to tady proto, ze produkt uz muze byt zalozeny, ale ja na instanci CustomerOnSA to nemam jak zjistit
            await _productService.GetMortgage(notification.CaseId, cancellationToken);
            _logger.LogInformation($"Product already exist for CaseId #{notification.CaseId}");
            return;
        }
        catch { }

        // detail SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(notification.SalesArrangementId, cancellationToken);
        if (!saInstance.OfferId.HasValue)
            throw new CisValidationException($"SalesArrangement #{notification.SalesArrangementId} is not bound to Offer");

        // detail offer
        var offerInstance = await _offerService.GetMortgageOffer(saInstance.OfferId.Value, cancellationToken);

        // zjistit, zda existuje customer v konsDb
        await _createOrUpdateCustomerKonsDb.CreateOrUpdate(notification.CustomerIdentifiers, cancellationToken);

        // vytovrit produkt - musi se zalozit pred klientem!
        var request = new _Product.CreateMortgageRequest
        {
            CaseId = notification.CaseId,
            Mortgage = offerInstance.ToDomainServiceRequest(mpId.Value)
        };
        var result = await _productService.CreateMortgage(request, cancellationToken);
        _bag.Add(CreateMortgageCaseRollback.BagKeyProductId, result);

        _logger.EntityCreated(nameof(_Product.CreateMortgageRequest), result);

        await _productService.CreateContractRelationship(mpId.Value, result, (int)CustomerRoles.Debtor, cancellationToken);
    }

    private readonly Infrastructure.Services.CreateOrUpdateCustomerKonsDb.CreateOrUpdateCustomerKonsDbService _createOrUpdateCustomerKonsDb;
    private readonly IRollbackBag _bag;
    private readonly DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IProductServiceClient _productService;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        Infrastructure.Services.CreateOrUpdateCustomerKonsDb.CreateOrUpdateCustomerKonsDbService createOrUpdateCustomerKonsDb,
        IRollbackBag bag,
        DomainServices.CustomerService.Clients.ICustomerServiceClient customerService,
        IOfferServiceClient offerService,
        ISalesArrangementServiceClient salesArrangementService,
        IProductServiceClient productService,
        ILogger<CreateProductHandler> logger)
    {
        _createOrUpdateCustomerKonsDb = createOrUpdateCustomerKonsDb;
        _bag = bag;
        _customerService = customerService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _productService = productService;
        _logger = logger;
    }
}
