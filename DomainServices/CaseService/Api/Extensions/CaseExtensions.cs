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
            TaskIdSb = int.Parse(dict["ukol_id"], CultureInfo.InvariantCulture),
            TaskId = int.Parse(dict["ukol_sada"], CultureInfo.InvariantCulture),
            CreatedOn = DateTime.Parse(dict["ukol_dat_start_proces"], CultureInfo.InvariantCulture),
            TaskTypeId = int.Parse(dict["ukol_typ_noby"], CultureInfo.InvariantCulture),
            TaskTypeName = dict["ukol_nazev_noby"],
            TaskSubtypeName = dict["ukol_oznaceni_noby"],
            ProcessId = int.Parse(dict["ukol_top_proces_sada"], CultureInfo.InvariantCulture),
            ProcessNameShort = dict["ukol_top_proces_nazev_noby"],
            StateIdSb = int.Parse(dict["ukol_stav_poz"], CultureInfo.InvariantCulture),
            Cancelled = Convert.ToBoolean(Convert.ToInt16(dict["ukol_stornovano"], CultureInfo.InvariantCulture))
        };

        task.PhaseTypeId = GetPhaseTypeId(task.TaskTypeId, dict);
        task.SignatureType = GetSignatureType(task.TaskTypeId, dict);

        return task;
    }

    public static ProcessTask ToProcessTask(this Ext1.EasSimulationHTWrapper.WFS_FindItem easTask)
    {
        var dict = easTask.task.ToDictionary(i => i.mtdt_def, i => i.mtdt_val);

        return new ProcessTask
        {
            ProcessIdSB = int.Parse(dict["ukol_id"], CultureInfo.InvariantCulture),
            ProcessId = int.Parse(dict["ukol_sada"], CultureInfo.InvariantCulture),
            CreatedOn = DateTime.Parse(dict["ukol_dat_start_proces"], CultureInfo.InvariantCulture),
            ProcessNameLong = dict["ukol_proces_nazev_noby"],
            StateName = dict["ukol_proces_oznacenie_noby"]
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

    private static int GetPhaseTypeId(int taskTypeId, IReadOnlyDictionary<string, string> dict)
    {
        return taskTypeId switch
        {
            1 => int.Parse(dict["ukol_dozadani_stav"], CultureInfo.InvariantCulture),
            2 => int.Parse(dict["ukol_overeni_ic_stav"], CultureInfo.InvariantCulture),
            6 => int.Parse(dict["ukol_podpis_stav"], CultureInfo.InvariantCulture),
            3 or 4 or 7 or 8 => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(taskTypeId), taskTypeId, null)
        };
    }

    private static string GetSignatureType(int taskTypeId, IReadOnlyDictionary<string, string> dict)
    {
        if (taskTypeId != 6)
            return "unknown";

        var signatureTypeId = int.Parse(dict["ukol_podpis_dokument_metoda"], CultureInfo.InvariantCulture);

        return signatureTypeId switch
        {
            1 => "paper",
            2 => "digital",
            _ => "unknown"
        };
    }
}