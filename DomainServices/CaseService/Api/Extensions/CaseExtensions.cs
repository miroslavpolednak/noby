using DomainServices.CaseService.Contracts;
using Ext1 = ExternalServices.EasSimulationHT.V1;

namespace DomainServices.CaseService.Api;

internal static class CaseExtensions
{
    public static WorkflowTask ToWorkflowTask(this Ext1.EasSimulationHTWrapper.WFS_FindItem easTask)
    {
        var dict = easTask.task.ToDictionary(i => i.mtdt_def, i => i.mtdt_val);

        var task = new WorkflowTask
        {
            TaskId = int.Parse(dict["ukol_id"], CultureInfo.InvariantCulture),
            TaskProcessId = int.Parse(dict["ukol_sada"], CultureInfo.InvariantCulture),
            TypeId = int.Parse(dict["ukol_typ"], CultureInfo.InvariantCulture),
            Name = dict["ukol_nazov"],
            CreatedOn = DateTime.Parse(dict["ukol_dat_start_proces"], CultureInfo.InvariantCulture),
            StateId = int.Parse(dict["ukol_stav_poz"], CultureInfo.InvariantCulture),
        };

        return task;
    }

    public static ActiveTask ToUpdateTaskItem(this WorkflowTask workflowTask)
    {
        return new ActiveTask
        {
            TaskProcessId = workflowTask.TaskProcessId,
            TypeId = workflowTask.TypeId
        };
    }
}