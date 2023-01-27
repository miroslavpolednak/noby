using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using _Cu = DomainServices.CustomerService.Contracts;
using DomainServices.OfferService.Clients;
using CIS.Infrastructure.gRPC.CisTypes;
using _Product = DomainServices.ProductService.Contracts;
using CIS.Infrastructure.CisMediatR.Rollback;
using NOBY.Api.Endpoints.Offer.CreateMortgageCase;
using MediatR;
using System.Threading;

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
        var kbIdentity = notification.CustomerIdentifiers!.First(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
        _Cu.CustomerDetailResponse? konsDbCustomer = null;
        try
        {
            //TODO rollbackovat i vytvoreni klienta?
            konsDbCustomer = await _customerService.GetCustomerDetail(mpIdentity!, cancellationToken);
        }
        catch (CisNotFoundException) // klient nenalezen v konsDb
        {
        }

        // klient nenalezen v konsDb, zaloz ho tam
        if (konsDbCustomer is null)
        {
            await createClientInKonsDb(kbIdentity, mpIdentity!, cancellationToken);
        }
        // ma klient v konsDb KB identitu? pokud ne, tak ho updatuj
        else if (konsDbCustomer.Identities.Any(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb))
        {
            await updateClientInKonsDb(konsDbCustomer, mpIdentity, kbIdentity, cancellationToken);
        }

        // vytovrit produkt
        var request = new _Product.CreateMortgageRequest
        {
            CaseId = notification.CaseId,
            Mortgage = offerInstance.ToDomainServiceRequest(mpId.Value)
        };
        var result = await _productService.CreateMortgage(request, cancellationToken);
        _bag.Add(CreateMortgageCaseRollback.BagKeyProductId, result);

        _logger.EntityCreated(nameof(_Product.CreateMortgageRequest), result);
    }

    private async Task updateClientInKonsDb(_Cu.CustomerDetailResponse originalClient, Identity mpIdentity, Identity kbIdentity, CancellationToken cancellationToken)
    {
        var request = new _Cu.UpdateCustomerRequest
        {
            Identities =
            {
                mpIdentity,
                kbIdentity
            },
            Mandant = Mandants.Mp,
            IdentificationDocument = originalClient.IdentificationDocument,
            NaturalPerson = originalClient.NaturalPerson
        };
        if (originalClient.Addresses is not null)
            request.Addresses.AddRange(originalClient.Addresses);
        if (originalClient.Contacts is not null)
            request.Contacts.AddRange(originalClient.Contacts);

        await _customerService.UpdateCustomer(request, cancellationToken);
    }

    private async Task createClientInKonsDb(Identity kbIdentity, Identity mpIdentity, CancellationToken cancellationToken)
    {
        // pokud neexistuje customer v konsDb, tak ho vytvor
        var customerDetail = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);

        // zalozit noveho klienta
        var request = new _Cu.CreateCustomerRequest
        {
            Identities =
            {
                mpIdentity,
                kbIdentity
            },
            HardCreate = true,
            NaturalPerson = customerDetail.NaturalPerson,
            Mandant = Mandants.Mp
        };

        if (customerDetail.IdentificationDocument is not null)
            request.IdentificationDocument = new _Cu.IdentificationDocument
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
            request.Addresses.Add(customerDetail.Addresses.Where(x => x.AddressTypeId == 1).Select(x => new GrpcAddress
            {
                Street = x.Street,
                City = x.City,
                AddressTypeId = x.AddressTypeId,
                HouseNumber = x.HouseNumber,
                StreetNumber = x.StreetNumber,
                EvidenceNumber = x.EvidenceNumber,
                Postcode = x.Postcode
            }));

        await _customerService.CreateCustomer(request, cancellationToken);
    }

    private readonly IRollbackBag _bag;
    private readonly DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IProductServiceClient _productService;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        IRollbackBag bag,
        DomainServices.CustomerService.Clients.ICustomerServiceClient customerService,
        IOfferServiceClient offerService,
        ISalesArrangementServiceClient salesArrangementService,
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
