using DomainServices.CaseService.Clients.v1;
using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Services.WorkflowTask;

public static class WorkflowTaskByTaskId
{
    public static async Task<WorkflowTaskByTaskIdResult> GetTaskByTaskId(this ICaseServiceClient caseService, long caseId, long taskId, CancellationToken cancellationToken)
    {
        var tasks = await caseService.GetTaskList(caseId, cancellationToken);

        var requestedTask = tasks.FirstOrDefault(t => t.TaskId == taskId) ?? throw new NobyValidationException($"Task {taskId} not found.");

        return new WorkflowTaskByTaskIdResult
        {
            Task = requestedTask,
            TaskList = tasks
        };
    }

    public class WorkflowTaskByTaskIdResult
    {
        public required DomainServices.CaseService.Contracts.WorkflowTask Task { get; init; }

        public required List<DomainServices.CaseService.Contracts.WorkflowTask> TaskList { get; init; }

        public int TaskIdSb => Task.TaskIdSb;
    }
}