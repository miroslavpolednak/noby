using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Services;

internal interface IActiveTasksService
{
    Task SyncActiveTasks(long caseId, List<WorkflowTask> tasks, CancellationToken cancellation);
    Task UpdateActiveTaskByTaskIdSb(long caseId, GetTaskDetailResponse taskDetail, CancellationToken cancellationToken);
    Task UpdateActiveTaskByTaskIdSb(long caseId, int taskIdSb, CancellationToken cancellationToken);
}