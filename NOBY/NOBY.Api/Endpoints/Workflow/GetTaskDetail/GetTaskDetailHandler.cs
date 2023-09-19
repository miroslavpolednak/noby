using CIS.Core.Security;
using DomainServices.CaseService.Clients;

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

        if ((task.TaskTypeId is 6 or 8) && !_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningView))
        {
            throw new CisAuthorizationException("Task detail manage permission missing");
        }
        else if (!(task.TaskTypeId is 6 or 8) && !_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_OtherView))
        {
            throw new CisAuthorizationException("Task detail manage permission missing");
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
    private static int[] _allowedTaskTypeIds = { 1, 2, 3, 6, 7 };

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
