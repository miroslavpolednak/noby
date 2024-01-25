
using DomainServices.CaseService.Clients;
using NOBY.Services.WorkflowTask;

namespace NOBY.Api.Endpoints.Workflow.GetCurrentHandoverTask;

public class GetCurrentHandoverTaskHandler : IRequestHandler<GetCurrentHandoverTaskRequest, GetCurrentHandoverTaskResponse>
{
    private readonly ICaseServiceClient _caseService;
    private readonly IWorkflowTaskService _workflowTaskService;

    public GetCurrentHandoverTaskHandler(
        ICaseServiceClient caseService,
        IWorkflowTaskService workflowTaskService)
    {
        _caseService = caseService;
        _workflowTaskService = workflowTaskService;
    }

    public async Task<GetCurrentHandoverTaskResponse> Handle(GetCurrentHandoverTaskRequest request, CancellationToken cancellationToken)
    {
        var task = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
            .Find(t => t.TaskTypeId == 7 && t.StateIdSb != 30)
            ?? throw new CisNotFoundException(NobyValidationException.DefaultExceptionCode, "Task in correct state not exist for specified CaseId");

        var (taskDto, taskDetailDto, documents) = await _workflowTaskService.GetTaskDetail(request.CaseId, task.TaskIdSb, cancellationToken);

        return new()
        {
            TaskDetail = taskDetailDto,
            Task = taskDto,
            Documents = documents
        };
    }
}
