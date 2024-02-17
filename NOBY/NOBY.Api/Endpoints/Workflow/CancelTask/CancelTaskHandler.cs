using CIS.Core.Security;
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
            || (task.TaskObject?.TaskTypeId == 9 && task.TaskObject?.PhaseTypeId != 1))
        {
            throw new NobyValidationException(90032, "TaskTypeId not allowed");
        }

        // cancel task in SB
        await _caseService.CancelTask(request.CaseId, request.TaskIdSB, cancellationToken);

        // cancel SA in NOBY
        if (task.TaskObject?.TaskTypeId == 9)
        {
            await _salesArrangementService.UpdateSalesArrangementState(1, (int)SalesArrangementStates.Cancelled, cancellationToken);
        }
    }

    private static int[] _allowedTypeIds = [ 2, 3, 9 ];

    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public CancelTaskHandler(ICaseServiceClient caseService, ICurrentUserAccessor currentUserAccessor, ISalesArrangementServiceClient salesArrangementService)
    {
        _caseService = caseService;
        _currentUserAccessor = currentUserAccessor;
        _salesArrangementService = salesArrangementService;
    }
}
