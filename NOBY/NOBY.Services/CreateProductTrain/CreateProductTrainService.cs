using Microsoft.Extensions.Logging;
using NOBY.Services.CreateProductTrain.Handlers;

namespace NOBY.Services.CreateProductTrain;

[ScopedService, AsImplementedInterfacesService]
internal sealed class CreateProductTrainService
    : ICreateProductTrainService
{
    public async Task RunAll(
        long caseId,
        int salesArrangementId,
        int customerOnSAId,
        IEnumerable<SharedTypes.GrpcTypes.Identity>? customerIdentifiers,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting CreateProductTrainService");

        await _updateCustomer.Run(caseId, customerOnSAId, cancellationToken);

        await _product.Run(caseId, salesArrangementId, customerOnSAId, customerIdentifiers, cancellationToken);

        long? mpId = customerIdentifiers?.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp)?.IdentityId;
        long? kbId = customerIdentifiers?.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)?.IdentityId;
        if (mpId.HasValue && kbId.HasValue)
        {
            await _createRiskBusinessCase.Run(salesArrangementId, cancellationToken);
        }
        else
        {
            _logger.LogInformation($"CreateRiskBusinessCaseService for CaseId #{caseId} not proceeding / missing MP ID");
        }
        
        _logger.LogDebug("CreateProductTrainService finished");
    }

    public async Task CreateRiskBusinessCaseAndUpdateSalesArrangement(DomainServices.SalesArrangementService.Contracts.SalesArrangement saInstance, CancellationToken cancellationToken)
    {
        var result = await _createRiskBusinessCase.Run(saInstance.SalesArrangementId, cancellationToken);
        if (result is not null)
        {
            saInstance.RiskBusinessCaseId = result.Value.RiskBusinessCaseId;
            saInstance.RiskSegment = result.Value.RiskSegment;
            saInstance.LoanApplicationDataVersion = result.Value.LoanApplicationDataVersion;
        }
    }

    private readonly ILogger<CreateProductTrainService> _logger;
    private readonly CreateProduct _product;
    private readonly UpdateCustomerOnCase _updateCustomer;
    private readonly CreateRiskBusinessCase _createRiskBusinessCase;

    public CreateProductTrainService(CreateProduct createProduct, UpdateCustomerOnCase updateCustomer, CreateRiskBusinessCase createRiskBusinessCase, ILogger<CreateProductTrainService> logger)
    {
        _logger = logger;
        _updateCustomer = updateCustomer;
        _createRiskBusinessCase = createRiskBusinessCase;
        _product = createProduct;
    }
}
