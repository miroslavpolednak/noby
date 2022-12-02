using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Cu = DomainServices.CustomerService.Contracts;
using DomainServices.OfferService.Clients;
using CIS.Infrastructure.gRPC.CisTypes;
using _Product = DomainServices.ProductService.Contracts;
using CIS.Infrastructure.MediatR.Rollback;
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
        var customerDetail = ServiceCallResult.ResolveAndThrowIfError<_Cu.CustomerDetailResponse>(await _customerService.GetCustomerDetail(notification.CustomerIdentifiers!.First(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb), cancellationToken));

        // zalozit noveho klienta
        var createCustomerRequest = new _Cu.CreateCustomerRequest
        {
            Identity = mpIdentity,
            HardCreate = true,
            NaturalPerson = customerDetail.NaturalPerson
        };
        if (customerDetail.IdentificationDocument is not null)
            createCustomerRequest.IdentificationDocument = new _Cu.IdentificationDocument
            {
                IssuedBy = customerDetail.IdentificationDocument.IssuedBy,
                IssuedOn = customerDetail.IdentificationDocument.IssuedOn,
                IssuingCountryId = customerDetail.IdentificationDocument.IssuingCountryId,
                Number = customerDetail.IdentificationDocument.Number,
                RegisterPlace = customerDetail.IdentificationDocument.RegisterPlace,
                IdentificationDocumentTypeId = customerDetail.IdentificationDocument.IdentificationDocumentTypeId,
                ValidTo = customerDetail.IdentificationDocument.ValidTo
            };
        if (customerDetail.Addresses is not null && customerDetail.Addresses.Any())
            createCustomerRequest.Addresses.Add(customerDetail.Addresses.Where(x => x.AddressTypeId == 1).Select(x => new GrpcAddress
            {
                Street = x.Street,
                City = x.City,
                AddressTypeId = x.AddressTypeId,
                HouseNumber = x.HouseNumber,
                StreetNumber = x.StreetNumber,
                EvidenceNumber = x.EvidenceNumber,
                Postcode = x.Postcode
            }));

        try
        {
            await _customerService.CreateCustomer(createCustomerRequest, cancellationToken);
        }
        catch (CisAlreadyExistsException)
        {
            // tise spolknout -> klient existuje a my jsme spokojeni
        }
        catch (Exception ex)
        {
            _logger.LogInformation("MpHome create client failed", ex);
            throw new CisConflictException(0, "MpHome client can't be created");
        }

        var request = new _Product.CreateMortgageRequest
        {
            CaseId = notification.CaseId,
            Mortgage = offerInstance.ToDomainServiceRequest(mpId.Value)
        };

        var result = ServiceCallResult.ResolveAndThrowIfError<_Product.ProductIdReqRes>(await _productService.CreateMortgage(request, cancellationToken));
        _bag.Add(CreateMortgageCaseRollback.BagKeyProductId, result.ProductId);
        //TODO rollbackovat i vytvoreni klienta?

        _logger.EntityCreated(nameof(_Product.Product), result.ProductId);
    }

    private readonly IRollbackBag _bag;
    private readonly DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService;
    private readonly IOfferServiceClients _offerService;
    private readonly ISalesArrangementServiceClients _salesArrangementService;
    private readonly IProductServiceClient _productService;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        IRollbackBag bag,
        DomainServices.CustomerService.Clients.ICustomerServiceClient customerService,
        IOfferServiceClients offerService,
        ISalesArrangementServiceClients salesArrangementService,
        IProductServiceClient productService,
        ILogger<CreateProductHandler> logger)
    {
        _bag = bag;
        _customerService = customerService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _productService = productService;
        _logger = logger;
    }
}
