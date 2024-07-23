using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.OfferService.Clients.v1;
using SharedTypes.GrpcTypes;
using _Product = DomainServices.ProductService.Contracts;
using CIS.Infrastructure.CisMediatR.Rollback;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;

namespace NOBY.Services.CreateProductTrain.Handlers;

[ScopedService, SelfService]
internal sealed class CreateProduct
{
    public async Task Run(
        long caseId,
        int salesArrangementId,
        int customerOnSAId,
        IEnumerable<Identity>? customerIdentifiers,
        CancellationToken cancellationToken)
    {
        var mpIdentity = customerIdentifiers?.GetMpIdentityOrDefault();
        long? mpId = mpIdentity?.IdentityId;
        if (!mpId.HasValue)
        {
            _logger.LogInformation("CreateProductHandler for CaseId #{CaseId} not proceeding / missing MP ID", caseId);
            return; // nema modre ID, nezajima me
        }

        try
        {
            // je to tady proto, ze produkt uz muze byt zalozeny, ale ja na instanci CustomerOnSA to nemam jak zjistit
            await _productService.GetMortgage(caseId, cancellationToken);
            _logger.LogInformation("Product already exist for CaseId #{CaseId}", caseId);
            return;
        }
        catch { }

        // detail SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
        if (!saInstance.OfferId.HasValue)
            throw new CisValidationException($"SalesArrangement #{salesArrangementId} is not bound to Offer");

        // detail offer
        var offerInstance = await _offerService.GetOffer(saInstance.OfferId.Value, cancellationToken);

        // zjistit, zda existuje customer v konsDb
        await _createOrUpdateCustomerKonsDb.CreateOrUpdate(customerIdentifiers!, cancellationToken);

        //ContractNumber
        var contractNumberResponse = await _salesArrangementService.SetContractNumber(salesArrangementId, customerOnSAId, cancellationToken);

        // vytovrit produkt - musi se zalozit pred klientem!
        var request = new _Product.CreateMortgageRequest
        {
            CaseId = caseId,
            Mortgage = offerInstance.ToDomainServiceRequest(mpId.Value, contractNumberResponse.ContractNumber)
        };

        request.Mortgage.CaseOwnerUserCurrentId = saInstance.Created.UserId;

        var result = await _productService.CreateMortgage(request, cancellationToken);
        _bag.Add("ProductId", result);

        await _salesArrangementService.UpdatePcpId(salesArrangementId, result.PcpId, cancellationToken);

        _logger.EntityCreated(nameof(_Product.CreateMortgageRequest), result.ProductId);
    }

    private readonly CreateOrUpdateCustomerKonsDb.CreateOrUpdateCustomerKonsDbService _createOrUpdateCustomerKonsDb;
    private readonly IRollbackBag _bag;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IProductServiceClient _productService;
    private readonly ILogger<CreateProduct> _logger;

    public CreateProduct(
        CreateOrUpdateCustomerKonsDb.CreateOrUpdateCustomerKonsDbService createOrUpdateCustomerKonsDb,
        IRollbackBag bag,
        IOfferServiceClient offerService,
        ISalesArrangementServiceClient salesArrangementService,
        IProductServiceClient productService,
        ILogger<CreateProduct> logger)
    {
        _createOrUpdateCustomerKonsDb = createOrUpdateCustomerKonsDb;
        _bag = bag;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _productService = productService;
        _logger = logger;
    }
}
