using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Workflow.GetTaskDetail;

internal sealed class GetTaskDetailHandler
    : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var task = await _workflowTaskService.LoadAndCheckIfTaskExists(request.CaseId, request.TaskId, cancellationToken);

        if (!_allowedTaskTypeIds.Contains(task.TaskTypeId))
        {
            throw new NobyValidationException(90032, "TaskTypeId not allowed");
        }

        if (task.TaskTypeId is (int)WorkflowTaskTypes.Signing)
        {
            // ProcessTypeId: Hlavní úvěrový proces ==1, Změnový proces == 2
            if (task.ProcessTypeId is ((int)WorkflowProcesses.Main or (int)WorkflowProcesses.Change) && !_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningView))
            {
                throw new CisAuthorizationException("Task detail view permission missing");
            }
            // zde by měl být jen Retenční proces == 3
            else if (task.ProcessTypeId is not ((int)WorkflowProcesses.Main or (int)WorkflowProcesses.Change) && !_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_RefinancingSigningView))
            {
                throw new CisAuthorizationException("Task detail view permission missing");
            }
        }
        else if (task.TaskTypeId is not (int)WorkflowTaskTypes.Signing)
        {
            // ProcessTypeId: Hlavní úvěrový proces ==1, Změnový proces == 2
            if (task.ProcessTypeId is ((int)WorkflowProcesses.Main or (int)WorkflowProcesses.Change) && !_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_OtherView))
            {
                throw new CisAuthorizationException("Task detail view permission missing");
            }
            // zde by měl být jen Retenční proces == 3
            else if (task.ProcessTypeId is not ((int)WorkflowProcesses.Main or (int)WorkflowProcesses.Change) && !_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_RefinancingOtherView))
            {
                throw new CisAuthorizationException("Task detail view permission missing");
            }
        }

        var (taskDto, taskDetailDto, documents) = await _workflowTaskService.GetTaskDetail(request.CaseId, task.TaskIdSb, cancellationToken);

        return new GetTaskDetailResponse
        {
            TaskDetail = taskDetailDto,
            Task = taskDto,
            Documents = documents
        };
    }

    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly Services.WorkflowTask.IWorkflowTaskService _workflowTaskService;
    private static int[] _allowedTaskTypeIds =
        [
            (int)WorkflowTaskTypes.Dozadani,
            (int)WorkflowTaskTypes.PriceException,
            (int)WorkflowTaskTypes.Consultation,
            (int)WorkflowTaskTypes.Signing,
            (int)WorkflowTaskTypes.PredaniNaSpecialitu
        ];

    public GetTaskDetailHandler(
        ICurrentUserAccessor currentUserAccessor,
        Services.WorkflowTask.IWorkflowTaskService workflowTaskService)
    {
        _currentUserAccessor = currentUserAccessor;
        _workflowTaskService = workflowTaskService;
    }
}
