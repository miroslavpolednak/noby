using CIS.Core.Security;
using DomainServices.CaseService.Clients;

namespace NOBY.Api.Endpoints.Workflow.CancelTask;

internal sealed class CancelTaskHandler
    : IRequestHandler<CancelTaskRequest>
{
    public async Task Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        // jen check jestli case existuje
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);

        var task = await _caseService.GetTaskDetail(request.TaskIdSB, cancellationToken);
        if (!_allowedTypeIds.Contains(task.TaskObject?.TaskTypeId ?? 0) || task.TaskObject?.TaskIdSb == 30)
        {
            throw new NobyValidationException(90032, "TaskTypeId not allowed");
        }
        WorkflowHelpers.ValidateTaskManagePermission(
            task.TaskObject?.TaskTypeId,
            task.TaskObject?.SignatureTypeId,
            task.TaskObject?.PhaseTypeId,
            task.TaskObject?.ProcessTypeId,
            _currentUserAccessor);

        await _caseService.CancelTask(request.CaseId, request.TaskIdSB, cancellationToken);
    }

    private static int[] _allowedTypeIds = [ 2, 3 ];

    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ICaseServiceClient _caseService;

    public CancelTaskHandler(ICaseServiceClient caseService, ICurrentUserAccessor currentUserAccessor)
    {
        _caseService = caseService;
        _currentUserAccessor = currentUserAccessor;
    }
}
