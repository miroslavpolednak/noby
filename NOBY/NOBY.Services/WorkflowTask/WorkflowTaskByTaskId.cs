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

    public static async Task<WorkflowProcessByProcessIdResult> GetProcessByProcessId(this ICaseServiceClient caseService, long caseId, long processId, CancellationToken cancellationToken)
    {
        var processes = await caseService.GetProcessList(caseId, cancellationToken);

        var requestedProcess = processes.FirstOrDefault(t => t.ProcessId == processId) ?? throw new NobyValidationException($"Process {processId} not found.");

        return new WorkflowProcessByProcessIdResult
        {
            Process = requestedProcess,
            ProcessList = processes
        };
    }

    public class WorkflowTaskByTaskIdResult
    {
        public required DomainServices.CaseService.Contracts.WorkflowTask Task { get; init; }

        public required List<DomainServices.CaseService.Contracts.WorkflowTask> TaskList { get; init; }

        public int TaskIdSb => Task.TaskIdSb;
    }

    public class WorkflowProcessByProcessIdResult
    {
        public required DomainServices.CaseService.Contracts.ProcessTask Process { get; init; }

        public required IList<DomainServices.CaseService.Contracts.ProcessTask> ProcessList { get; init; }

        public int ProcessIdSb => Process.ProcessIdSb;
    }
}