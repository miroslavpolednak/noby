using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using NOBY.Services.WorkflowTask;

namespace NOBY.Api.Endpoints.Workflow.GetTaskDetail;

internal sealed class GetTaskDetailHandler(
    ICurrentUserAccessor _currentUserAccessor,
    ICaseServiceClient _caseService,
    IWorkflowTaskService _workflowTaskService)
        : IRequestHandler<GetTaskDetailRequest, WorkflowGetTaskDetailResponse>
{
    public async Task<WorkflowGetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var taskResult = await _caseService.GetTaskByTaskId(request.CaseId, request.TaskId, cancellationToken);
        var task = taskResult.Task;

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

        return new WorkflowGetTaskDetailResponse
        {
            TaskDetail = taskDetailDto,
            Task = taskDto,
            Documents = documents
        };
    }

    private static readonly int[] _allowedTaskTypeIds =
        [
            (int)WorkflowTaskTypes.Dozadani,
            (int)WorkflowTaskTypes.PriceException,
            (int)WorkflowTaskTypes.Consultation,
            (int)WorkflowTaskTypes.Signing,
            (int)WorkflowTaskTypes.PredaniNaSpecialitu
        ];
}
