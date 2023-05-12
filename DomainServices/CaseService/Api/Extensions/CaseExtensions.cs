using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api;

internal static class CaseExtensions
{
    public static WorkflowTask ToWorkflowTask(this IReadOnlyDictionary<string, string> taskData)
    {
        var task = new WorkflowTask
        {
            TaskIdSb = taskData.GetInteger("ukol_id"),
            TaskId = taskData.GetLong("ukol_sada"),
            CreatedOn = taskData.GetDate("ukol_dat_start_proces"),
            TaskTypeId = taskData.GetInteger("ukol_typ_noby"),
            TaskTypeName = taskData.GetValueOrDefault("ukol_nazev_noby") ?? "",
            TaskSubtypeName = taskData.GetValueOrDefault("ukol_oznaceni_noby") ?? "",
            ProcessId = taskData.GetLong("ukol_top_proces_sada"),
            ProcessNameShort = taskData.GetValueOrDefault("ukol_top_proces_nazev_noby") ?? "",
            StateIdSb = taskData.GetInteger("ukol_stav_poz"),
            Cancelled = taskData.GetBoolean("ukol_stornovano"),
            PerformerLogin = taskData.GetValueOrDefault("ukol_op_zpracovatel") ?? ""
        };

        task.PhaseTypeId = GetPhaseTypeId(task.TaskTypeId, taskData);
        task.SignatureType = GetSignatureType(task.TaskTypeId, taskData);

        return task;
    }

    public static ProcessTask ToProcessTask(this IReadOnlyDictionary<string, string> taskData)
    {
        var process =  new ProcessTask
        {
            ProcessIdSb = taskData.GetInteger("ukol_id"),
            ProcessId = taskData.GetLong("ukol_sada"),
            CreatedOn = taskData.GetDate("ukol_dat_start_proces"),
            ProcessTypeId = taskData.GetInteger("ukol_proces_typ_noby"),
            ProcessNameLong = taskData.GetValueOrDefault("ukol_proces_nazev_noby") ?? "",
        };

        process.ProcessPhaseId = GetProcessPhaseId(process.ProcessTypeId, taskData);
        SetStateNameAndIndicator(process, taskData);

        return process;
    }

    public static TaskDetailItem ToTaskDetail(this IReadOnlyDictionary<string, string> taskData)
    {
        var taskDetail = new TaskDetailItem
        {
            ProcessNameLong = taskData.GetValueOrDefault("ukol_typ_proces_noby_oznaceni") ?? "",
            TaskDocumentIds = { (taskData.GetValueOrDefault("wfl_refobj_dokumenty") ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries) }
        };
        
        if (int.TryParse(taskData.GetValueOrDefault("ukol_typ_noby"), out int taskType))
        {
            switch (taskType)
            {
                case 1:
                    taskDetail.Request = new()
                    {
                        SentToCustomer = taskData.GetValueOrDefault("ukol_dozadani_prijemce_typ") == "1",
                        OrderId = taskData.GetValueOrDefault("ukol_dozadani_typ") == "5" ? taskData.GetNInteger("ukol_dozadani_order_id") : null
                    };
                    break;

                case 6:
                    taskDetail.Signing = new()
                    {
                        FormId = taskData.GetValueOrDefault("ukol_podpis_dokument_form_id") ?? "",
                        Expiration = taskData.GetValueOrDefault("ukol_podpis_lhuta_do") ?? "",
                        DocumentForSigning = taskData.GetValueOrDefault("ukol_podpis_dokument_ep_id") ?? "",
                        ProposalForEntry = taskData.GetValueOrDefault("ukol_podpis_prilohy_ep_id")
                    };
                    break;

                case 3 when _allowedConsultationTypes.Contains(taskData.GetNInteger("ukol_konzultace_oblast").GetValueOrDefault()):
                    taskDetail.ConsultationData = new()
                    {
                        OrderId = taskData.GetNInteger("ukol_konzultace_order_id")
                    };
                    break;
            }
        }

        return taskDetail;
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

    private static void SetStateNameAndIndicator(ProcessTask process, IReadOnlyDictionary<string, string> taskData)
    {
        if (taskData.GetBoolean("ukol_stornovano"))
        {
            process.StateName = "ZRUŠENO";
            process.StateIndicator = 2;

            return;
        }

        if (taskData.GetInteger("ukol_stav_poz") == 30)
        {
            process.StateName = "DOKONČENO";
            process.StateIndicator = 3;

            return;
        }

        process.StateName = taskData.GetValueOrDefault("ukol_proces_oznacenie_noby") ?? "";
        process.StateIndicator = 1;
    }

    private static int GetProcessPhaseId(int processTypeId, IReadOnlyDictionary<string, string> taskData)
    {
        return processTypeId switch
        {
            1 => taskData.GetInteger("ukol_faze_uv_procesu"),
            2 => taskData.GetInteger("ukol_faze_zm_procesu"),
            3 => taskData.GetInteger("ukol_faze_rt_procesu"),
            -1 => -1,
            _ => throw new ArgumentOutOfRangeException(nameof(processTypeId), processTypeId, null)
        };
    }

    private static int[] _allowedConsultationTypes = new[] { 1, 7 };
}