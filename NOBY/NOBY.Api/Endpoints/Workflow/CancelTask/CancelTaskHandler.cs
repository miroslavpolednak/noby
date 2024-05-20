using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Services.WorkflowTask;

namespace NOBY.Api.Endpoints.Workflow.CancelTask;

internal sealed class CancelTaskHandler
    : IRequestHandler<CancelTaskRequest>
{
    public async Task Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        // jen check jestli case existuje
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);

        var taskIdSb = request.TaskIdSB ?? (await _caseService.GetProcessByProcessId(request.CaseId, request.TaskId, cancellationToken)).ProcessIdSb;

        var task = await _caseService.GetTaskDetail(taskIdSb, cancellationToken);
        
        // overeni prav
        WorkflowHelpers.ValidateTaskManagePermission(
            task.TaskObject?.TaskTypeId,
            task.TaskObject?.SignatureTypeId,
            task.TaskObject?.PhaseTypeId,
            task.TaskObject?.ProcessTypeId,
            _currentUserAccessor);
        
        // overeni prav mimo spolecnou logiku
        if (!_allowedTypeIds.Contains(task.TaskObject?.TaskTypeId ?? 0) 
            || task.TaskObject?.TaskIdSb == 30
            || (task.TaskObject?.TaskTypeId == (int)WorkflowTaskTypes.Retention && task.TaskObject?.PhaseTypeId != 1))
        {
            throw new NobyValidationException(90032, "TaskTypeId not allowed");
        }

        // cancel task in SB
        await _caseService.CancelTask(request.CaseId, taskIdSb, cancellationToken);

        // cancel SA in NOBY
        if (task.TaskObject?.TaskTypeId == (int)WorkflowTaskTypes.Retention)
        {
            await cancelRetentionSalesArrangement(request.CaseId, task.TaskObject.ProcessId, cancellationToken);
        }
    }

    /// <summary>
    /// Stornovani SA v pripade retence
    /// </summary>
    private async Task cancelRetentionSalesArrangement(long caseId, long processId, CancellationToken cancellationToken)
    {
        // najit retencni SA
        var saList = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);
        var saId = saList.SalesArrangements.FirstOrDefault(t => t.ProcessId == processId)?.SalesArrangementId;

        if (saId.HasValue)
        {
            await _salesArrangementService.UpdateSalesArrangementState(saId.Value, (int)SalesArrangementStates.Cancelled, cancellationToken);
        }
    }

    // povolene typy tasku
    private static int[] _allowedTypeIds = 
        [
            (int)WorkflowTaskTypes.PriceException,
            (int)WorkflowTaskTypes.Consultation,
            (int)WorkflowTaskTypes.Retention,
            (int)WorkflowTaskTypes.ExtraPayment
        ];

    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ICaseServiceClient _caseService;
    private readonly IWorkflowTaskService _workflowTaskService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public CancelTaskHandler(ICaseServiceClient caseService, IWorkflowTaskService workflowTaskService, ICurrentUserAccessor currentUserAccessor, ISalesArrangementServiceClient salesArrangementService)
    {
        _caseService = caseService;
        _workflowTaskService = workflowTaskService;
        _currentUserAccessor = currentUserAccessor;
        _salesArrangementService = salesArrangementService;
    }
}
