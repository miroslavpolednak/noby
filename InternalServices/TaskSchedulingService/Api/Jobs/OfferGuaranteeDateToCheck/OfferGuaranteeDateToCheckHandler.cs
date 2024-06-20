using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.OfferGuaranteeDateToCheck;

/// <summary>
/// Original SalesArrangement.OfferGuaranteeDateToCheckHandler
/// </summary>
internal sealed class OfferGuaranteeDateToCheckHandler(
    ICaseServiceClient _caseService, 
    IMaintananceService _maintananceService, 
    ILogger<OfferGuaranteeDateToCheckHandler> _logger, 
    ISalesArrangementServiceClient _salesArrangementService)
    : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        var ids = await _maintananceService.GetOfferGuaranteeDateToCheck(cancellationToken);

        foreach (var flowSwitch in ids)
        {
            var taskList = await _caseService.GetTaskList(flowSwitch.CaseId, cancellationToken);
            var task = taskList.FirstOrDefault(t => t is { TaskTypeId: (int)WorkflowTaskTypes.PriceException, Cancelled: false });

            if (task is not null)
            {
                try
                {
                    await _caseService.CancelTask(flowSwitch.CaseId, task.TaskIdSb, cancellationToken);
                    _logger.OfferGuaranteeDateToCheckJobCancelTask(flowSwitch.CaseId, task.TaskIdSb);
                }
                catch
                {
                    // zhucelo to asi na neexistujicim tasku, ale to je jedno
                }
            }

            await _salesArrangementService.SetFlowSwitch(flowSwitch.SalesArrangementId, FlowSwitches.IsOfferGuaranteed, false, cancellationToken);

            _logger.OfferGuaranteeDateToCheckJobFinished(flowSwitch.SalesArrangementId);
        }
    }
}
