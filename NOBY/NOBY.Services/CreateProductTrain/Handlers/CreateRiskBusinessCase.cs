﻿using DomainServices.SalesArrangementService.Clients;
using Microsoft.Extensions.Logging;

namespace NOBY.Services.CreateProductTrain.Handlers;

/// <summary>
/// Zalozi RiskBusinessCase pro dany Sales Arrangement
/// </summary>
[ScopedService, SelfService]
internal sealed class CreateRiskBusinessCase
{
    public async Task Run(
        int salesArrangementId,
        CancellationToken cancellationToken)
    {
        //SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);

        if (string.IsNullOrEmpty(saInstance.RiskBusinessCaseId))
        {
            try
            {
                var riskCase = await _riskCaseProcessor.CreateOrUpdateRiskCase(salesArrangementId, cancellationToken);

                await _salesArrangementService.UpdateLoanAssessmentParameters(new DomainServices.SalesArrangementService.Contracts.UpdateLoanAssessmentParametersRequest
                {
                    SalesArrangementId = saInstance.SalesArrangementId,
                    CommandId = saInstance.CommandId,
                    RiskSegment = riskCase.RiskSegment
                }, cancellationToken);
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
    private readonly Services.RiskCaseProcessor.RiskCaseProcessorService _riskCaseProcessor;

    public CreateRiskBusinessCase(
        Services.RiskCaseProcessor.RiskCaseProcessorService riskCaseProcessor, 
        ISalesArrangementServiceClient salesArrangementService, 
        ILogger<CreateRiskBusinessCase> logger)
    {
        _salesArrangementService = salesArrangementService;
        _riskCaseProcessor = riskCaseProcessor;
        _logger = logger;
    }
}
