using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelCaseBySalesArrangementConditions;

/// <summary>
/// Original SalesArrangementService.CancelCase
/// </summary>
internal sealed class CancelCaseBySalesArrangementConditionsHandler(
    ICaseServiceClient _caseServiceClient, 
    ILogger<CancelCaseBySalesArrangementConditionsHandler> _logger, 
    DomainServices.SalesArrangementService.Clients.IMaintananceService _maintananceService)
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
                else if (CaseHelpers.IsCaseInState(CaseHelpers.AllExceptInProgressStates, (EnumCaseStates)caseInstance.State!))
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
}
