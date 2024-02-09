using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.CaseService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelCaseBySalesArrangementConditions;

/// <summary>
/// Original SalesArrangementService.CancelCase
/// </summary>
internal sealed class CancelCaseBySalesArrangementConditionsHandler
    : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        var caseIds = await _maintananceService.GetCancelCaseJobIds(cancellationToken);

        foreach (var caseId in caseIds)
        {
            try
            {
                var caseInstance = await _caseServiceClient.ValidateCaseId(caseId, false, cancellationToken);

                if (!caseInstance.Exists)
                {
                    _logger.CancelCaseJobSkipped(caseId, "Case does not exist");
                }
                else if (caseInstance.State != (int)CaseStates.InProgress)
                {
                    _logger.CancelCaseJobSkipped(caseId, $"CaseState is {caseInstance.State}");
                }
                else
                {
                    await _caseServiceClient.CancelCase(caseId, false, cancellationToken);

                    _logger.CancelCaseJobFinished(caseId);
                }
            }
            catch (Exception e)
            {
                _logger.CancelCaseJobFailed(caseId, e.Message, e);
            }
        }
    }

    private readonly DomainServices.SalesArrangementService.Clients.IMaintananceService _maintananceService;
    private readonly ICaseServiceClient _caseServiceClient;
    private readonly ILogger<CancelCaseBySalesArrangementConditionsHandler> _logger;

    public CancelCaseBySalesArrangementConditionsHandler(ICaseServiceClient caseServiceClient, ILogger<CancelCaseBySalesArrangementConditionsHandler> logger, DomainServices.SalesArrangementService.Clients.IMaintananceService maintananceService)
    {
        _caseServiceClient = caseServiceClient;
        _logger = logger;
        _maintananceService = maintananceService;
    }
}
