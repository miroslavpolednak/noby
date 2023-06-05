using DomainServices.CaseService.Clients;

namespace NOBY.Api.Endpoints.Workflow.GetTaskDetail;

internal sealed class GetTaskDetailHandler : IRequestHandler<GetTaskDetailRequest, GetTaskDetailResponse>
{
    private readonly Infrastructure.Services.WorkflowTask.IWorkflowTaskService _workflowTaskService;
    private readonly ICaseServiceClient _caseService;
    private static int[] _allowedTaskTypeIds = { 1, 2, 3, 6, 7 };
    
    public async Task<GetTaskDetailResponse> Handle(GetTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var taskList = await _caseService.GetTaskList(request.CaseId, cancellationToken);
        var task = taskList.FirstOrDefault(t => t.TaskId == request.TaskId)
            ?? throw new CisNotFoundException(90001, $"Task {request.TaskId} not found.");

        if (!_allowedTaskTypeIds.Contains(task.TaskTypeId))
        {
            throw new CisAuthorizationException();
        }
        
        var (taskDto, taskDetailDto, documents) = await _workflowTaskService.GetTaskDetail(request.CaseId, task.TaskIdSb, cancellationToken);

        return new GetTaskDetailResponse
        {
            TaskDetail = taskDetailDto,
            Task = taskDto,
            Documents = documents
        };
    }

    public GetTaskDetailHandler(
            Infrastructure.Services.WorkflowTask.IWorkflowTaskService workflowTaskService,
            ICaseServiceClient caseService)
    {
        _workflowTaskService = workflowTaskService;
        _caseService = caseService;
    }
}
