
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using NOBY.Services.WorkflowTask;

namespace NOBY.Api.Endpoints.Workflow.GetCurrentHandoverTask;

public class GetCurrentHandoverTaskHandler(
    ICaseServiceClient caseService,
    IWorkflowTaskService workflowTaskService,
    ICodebookServiceClient codebookService) : IRequestHandler<GetCurrentHandoverTaskRequest, GetCurrentHandoverTaskResponse>
{
    private readonly ICaseServiceClient _caseService = caseService;
    private readonly IWorkflowTaskService _workflowTaskService = workflowTaskService;
    private readonly ICodebookServiceClient _codebookService = codebookService;

    public async Task<GetCurrentHandoverTaskResponse> Handle(GetCurrentHandoverTaskRequest request, CancellationToken cancellationToken)
    {
        var task = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
            .Find(t => t.TaskTypeId == 7 && t.StateIdSb != 30);

        if (task is null)
        {
            return await CreateResponseManually(request, cancellationToken);
        }

        var (taskDto, taskDetailDto, documents) = await _workflowTaskService.GetTaskDetail(request.CaseId, task.TaskIdSb, cancellationToken);

        return new()
        {
            TaskDetail = taskDetailDto,
            Task = taskDto,
            Documents = documents
        };
    }

    private async Task<GetCurrentHandoverTaskResponse> CreateResponseManually(GetCurrentHandoverTaskRequest request, CancellationToken cancellationToken)
    {
        var workflowTypeName = (await _codebookService.WorkflowTaskTypes(cancellationToken)).Find(r => r.Id == 7)?.Name;
        var process = (await _caseService.GetProcessList(request.CaseId, cancellationToken)).FirstOrDefault(r => r.ProcessTypeId == 1);

        return new()
        {
            Task = new()
            {
                TaskTypeId = 7,
                TaskTypeName = workflowTypeName ?? string.Empty,
                ProcessId = process?.ProcessId ?? default

            },
            TaskDetail = new()
            {
                ProcessNameLong = process?.ProcessNameLong ?? string.Empty,
            }
        };

    }
}
