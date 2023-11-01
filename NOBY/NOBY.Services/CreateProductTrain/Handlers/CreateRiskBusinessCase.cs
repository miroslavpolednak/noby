using DomainServices.SalesArrangementService.Clients;
using Microsoft.Extensions.Logging;

namespace NOBY.Services.CreateProductTrain.Handlers;

/// <summary>
/// Zalozi RiskBusinessCase pro dany Sales Arrangement
/// </summary>
[ScopedService, SelfService]
internal sealed class CreateRiskBusinessCase
{
    public async Task Run(
        long caseId,
        int salesArrangementId,
        int customerOnSAId,
        IEnumerable<SharedTypes.GrpcTypes.Identity>? customerIdentifiers,
        CancellationToken cancellationToken)
    {
        //SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
        if (string.IsNullOrEmpty(saInstance.RiskBusinessCaseId))
        {
            try
            {
                await _createRiskBusiness.Create(caseId, salesArrangementId, customerOnSAId, customerIdentifiers, cancellationToken);
            }
            catch (CisValidationException ex)
            {
                _logger.LogInformation($"CreateRBC failed: {ex.FirstExceptionCode}; {ex.Message}");
            }
        }
        else // RBCID je jiz zalozene, ukonci flow
        {
            _logger.LogInformation($"SalesArrangement #{salesArrangementId} already contains RiskBusinessCaseId");
        }
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ILogger<CreateRiskBusinessCase> _logger;
    private readonly Services.CreateRiskBusinessCase.CreateRiskBusinessCaseService _createRiskBusiness;

    public CreateRiskBusinessCase(Services.CreateRiskBusinessCase.CreateRiskBusinessCaseService createRiskBusiness, ISalesArrangementServiceClient salesArrangementService, ILogger<CreateRiskBusinessCase> logger)
    {
        _salesArrangementService = salesArrangementService;
        _createRiskBusiness = createRiskBusiness;
        _logger = logger;
    }
}
