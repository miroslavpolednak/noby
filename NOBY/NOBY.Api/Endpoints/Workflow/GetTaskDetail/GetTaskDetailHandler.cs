using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;

namespace NOBY.Api.Endpoints.Workflow.GetTaskDetail;

internal sealed class GetTaskDetailHandler
    : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(request.CaseId, cancellationToken);
        var task = taskList.FirstOrDefault(t => t.TaskId == request.TaskId)
            ?? throw new NobyValidationException($"Task {request.TaskId} not found.");

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
    private readonly ICaseServiceClient _caseService;
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
        Services.WorkflowTask.IWorkflowTaskService workflowTaskService,
        ICaseServiceClient caseService)
    {
        _currentUserAccessor = currentUserAccessor;
        _workflowTaskService = workflowTaskService;
        _caseService = caseService;
    }
}
