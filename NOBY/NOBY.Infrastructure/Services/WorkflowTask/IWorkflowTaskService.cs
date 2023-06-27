namespace NOBY.Infrastructure.Services.WorkflowTask;

public interface IWorkflowTaskService
{
    Task<(Dto.Workflow.WorkflowTask Task, Dto.Workflow.WorkflowTaskDetail TaskDetail, List<Dto.Documents.DocumentsMetadata> Documents)> GetTaskDetail(
        long caseId,
        int taskIdSb,
        CancellationToken cancellationToken = default);
}
