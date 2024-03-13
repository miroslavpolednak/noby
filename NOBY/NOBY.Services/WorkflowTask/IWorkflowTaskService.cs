namespace NOBY.Services.WorkflowTask;

public interface IWorkflowTaskService
{
    Task<(Dto.Workflow.WorkflowTask Task, Dto.Workflow.WorkflowTaskDetail TaskDetail, List<Dto.Documents.DocumentsMetadata> Documents)> GetTaskDetail(
        long caseId,
        int taskIdSb,
        CancellationToken cancellationToken = default);

    Task<DomainServices.CaseService.Contracts.WorkflowTask> LoadAndCheckIfTaskExists(long caseId, long taskId, CancellationToken cancellationToken);
}
