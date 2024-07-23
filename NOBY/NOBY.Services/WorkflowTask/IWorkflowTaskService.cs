using NOBY.ApiContracts;

namespace NOBY.Services.WorkflowTask;

public interface IWorkflowTaskService
{
    Task<(SharedTypesWorkflowTask Task, SharedTypesWorkflowTaskDetail TaskDetail, List<SharedTypesDocumentsMetadata> Documents)> GetTaskDetail(
        long caseId,
        int taskIdSb,
        CancellationToken cancellationToken = default);
}
