using NOBY.ApiContracts;

namespace NOBY.Services.WorkflowTask;

public interface IWorkflowTaskServiceOld
{
    Task<(Dto.Workflow.WorkflowTask Task, Dto.Workflow.WorkflowTaskDetail TaskDetail, List<SharedTypesDocumentsMetadata> Documents)> GetTaskDetail(
        long caseId,
        int taskIdSb,
        CancellationToken cancellationToken = default);
}
