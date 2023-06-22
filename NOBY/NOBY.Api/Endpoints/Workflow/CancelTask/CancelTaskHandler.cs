using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Workflow.CancelTask;

internal sealed class CancelTaskHandler
    : IRequestHandler<CancelTaskRequest>
{
    public async Task Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        // jen check jestli case existuje
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);

        var task = await _caseService.GetTaskDetail(request.TaskIdSB, cancellationToken);
        if (_allowedTypeIds.Contains(task.TaskObject?.TaskTypeId ?? 0) || task.TaskObject?.TaskIdSb == 30)
        {
            throw new CisAuthorizationException();
        }

        await _caseService.CancelTask(request.TaskIdSB, cancellationToken);

        // set flow switches
        if (task.TaskObject?.TaskTypeId == 2)
        {
            var saId = await _salesArrangementService.GetProductSalesArrangement(request.CaseId, cancellationToken);
            await _salesArrangementService.SetFlowSwitches(saId, new()
            {
                new() { FlowSwitchId = (int)FlowSwitches.DoesWflTaskForIPExist, Value = false },
                new() { FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPApproved, Value = false },
                new() { FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPNotApproved, Value = false }
            }, cancellationToken);
        }
    }

    private static int[] _allowedTypeIds = new[] { 2, 3 };

    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public CancelTaskHandler(ICaseServiceClient caseService, ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
    }
}
