using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api;

internal static class CaseExtensions
{
    public static WorkflowTask ToWorkflowTask(this IReadOnlyDictionary<string, string> taskData)
    {
        var task = new WorkflowTask
        {
            TaskIdSb = taskData.GetInteger("ukol_id"),
            TaskId = taskData.GetInteger("ukol_sada"),
            CreatedOn = taskData.GetDate("ukol_dat_start_proces"),
            TaskTypeId = taskData.GetInteger("ukol_typ_noby"),
            TaskTypeName = taskData["ukol_nazev_noby"],
            TaskSubtypeName = taskData["ukol_oznaceni_noby"],
            ProcessId = taskData.GetInteger("ukol_top_proces_sada"),
            ProcessNameShort = taskData["ukol_top_proces_nazev_noby"],
            StateIdSb = taskData.GetInteger("ukol_stav_poz"),
            Cancelled = taskData.GetBoolean("ukol_stornovano")
        };

        task.PhaseTypeId = GetPhaseTypeId(task.TaskTypeId, taskData);
        task.SignatureType = GetSignatureType(task.TaskTypeId, taskData);

        return task;
    }

    public static ProcessTask ToProcessTask(this IReadOnlyDictionary<string, string> taskData)
    {
        return new ProcessTask
        {
            ProcessIdSB = taskData.GetInteger("ukol_id"),
            ProcessId = taskData.GetInteger("ukol_sada"),
            CreatedOn = taskData.GetDate("ukol_dat_start_proces"),
            ProcessNameLong = taskData["ukol_proces_nazev_noby"],
            StateName = taskData["ukol_proces_oznacenie_noby"]
        };
    }

    public static TaskDetailItem ToTaskDetail(this IReadOnlyDictionary<string, string> taskData)
    {
        return new TaskDetailItem
        {
            ProcessNameLong = taskData["ukol_typ_proces_noby_oznaceni"],
        };
    }

    public static ActiveTask ToUpdateTaskItem(this WorkflowTask workflowTask)
    {
        return new ActiveTask
        {
            TaskId = workflowTask.TaskId,
            TaskTypeId = workflowTask.TaskTypeId
        };
    }

    private static int GetPhaseTypeId(int taskTypeId, IReadOnlyDictionary<string, string> taskData)
    {
        return taskTypeId switch
        {
            1 => taskData.GetInteger("ukol_dozadani_stav"),
            2 => taskData.GetInteger("ukol_overeni_ic_stav"),
            6 => taskData.GetInteger("ukol_podpis_stav"),
            3 or 4 or 7 or 8 => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(taskTypeId), taskTypeId, null)
        };
    }

    private static string GetSignatureType(int taskTypeId, IReadOnlyDictionary<string, string> taskData)
    {
        if (taskTypeId != 6)
            return "unknown";

        var signatureTypeId = taskData.GetInteger("ukol_podpis_dokument_metoda");

        return signatureTypeId switch
        {
            1 => "paper",
            2 => "digital",
            _ => "unknown"
        };
    }
}