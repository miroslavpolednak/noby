using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.OfferGuaranteeDateToCheck;

/// <summary>
/// Original SalesArrangement.OfferGuaranteeDateToCheckHandler
/// </summary>
internal sealed class OfferGuaranteeDateToCheckHandler
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
                await _caseService.CancelTask(flowSwitch.CaseId, task.TaskIdSb, cancellationToken);
                _logger.OfferGuaranteeDateToCheckJobCancelTask(flowSwitch.CaseId, task.TaskIdSb);
            }

            await _salesArrangementService.SetFlowSwitch(flowSwitch.SalesArrangementId, FlowSwitches.IsOfferGuaranteed, false, cancellationToken);

            _logger.OfferGuaranteeDateToCheckJobFinished(flowSwitch.SalesArrangementId);
        }
    }

    private readonly ILogger<OfferGuaranteeDateToCheckHandler> _logger;
    private readonly DomainServices.SalesArrangementService.Clients.IMaintananceService _maintananceService;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public OfferGuaranteeDateToCheckHandler(ICaseServiceClient caseService, IMaintananceService maintananceService, ILogger<OfferGuaranteeDateToCheckHandler> logger, ISalesArrangementServiceClient salesArrangementService)
    {
        _caseService = caseService;
        _maintananceService = maintananceService;
        _logger = logger;
        _salesArrangementService = salesArrangementService;
    }
}
