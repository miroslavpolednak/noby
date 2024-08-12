using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CaseService.Api.Services;

internal static class ActiveTaskExtensions
{
    private static bool FlagIsActive(this WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem workflowTaskStatesItem) =>
        workflowTaskStatesItem.Flag == WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag.None;

    private static WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem GetWorkflowTaskState(
        this WorkflowTask workflowTask,
        IEnumerable<WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem> workflowTaskStates) =>
        workflowTaskStates.First(t => t.Id == workflowTask.StateIdSb);
    
    public static bool IsActive(
        this WorkflowTask workflowTask,
        IEnumerable<WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem> workflowTaskStates) =>
        (workflowTask.GetWorkflowTaskState(workflowTaskStates).FlagIsActive() && workflowTask.TaskTypeId != 2) ||
        workflowTask is { TaskTypeId: 2, PhaseTypeId: 1 };
}