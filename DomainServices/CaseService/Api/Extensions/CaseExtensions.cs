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

        task.PhaseTypeId = getPhaseTypeId(task.TaskTypeId, taskData);
        task.SignatureType = getSignatureType(task.TaskTypeId, taskData);

        return task;
    }

    public static ProcessTask ToProcessTask(this IReadOnlyDictionary<string, string> taskData)
    {
        return new ProcessTask
        {
            ProcessIdSb = taskData.GetInteger("ukol_id"),
            ProcessId = taskData.GetLong("ukol_sada"),
            CreatedOn = taskData.GetDate("ukol_dat_start_proces"),
            ProcessTypeId = taskData.GetInteger("ukol_proces_typ_noby"),
            ProcessNameLong = taskData.GetValueOrDefault("ukol_proces_nazev_noby") ?? "",
            StateName = getStateName(taskData),
            ProcessPhaseId = taskData.GetInteger("ukol_proces_typ_noby") switch
            {
                1 => taskData.GetInteger("ukol_faze_uv_procesu"),
                2 => taskData.GetInteger("ukol_faze_zm_procesu"),
                3 => taskData.GetInteger("ukol_faze_rt_procesu"),
                -1 => -1,
                _ => throw new CisArgumentException(0, taskData["ukol_proces_typ_noby"], "ProcessPhaseId")
            },
            StateIndicator = taskData.GetInteger("ukol_proces_typ_noby") switch
            {
                1 or 2 or 3 => getStateIndicator(taskData),
                _ => null
            }
        };
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

                // price exception
                case 2:
                    taskDetail.PriceException = new()
                    {
                        Expiration = taskData.GetDate("ukol_overeni_ic_sazba_dat_do"),
                        LoanInterestRate = new()
                        {
                            LoanInterestRate = taskData.GetValueOrDefault("ukol_overeni_ic_sazba_nabid") ?? "",
                            LoanInterestRateProvided = taskData.GetValueOrDefault("ukol_overeni_ic_sazba_vysled") ?? "",
                            LoanInterestRateAnnouncedType = taskData.GetInteger("ukol_overeni_ic_sazba_typ"),
                            LoanInterestRateDiscount = taskData.GetValueOrDefault("ukol_overeni_ic_sazba_sleva")
                        },
                        DecisionId = taskData.GetNInteger("ukol_overeni_ic_zpusob_reseni")
                    };
                    for (int i = 1; i < 5; i++)
                    {
                        if (string.IsNullOrEmpty(taskData.GetValueOrDefault($"ukol_overeni_ic_popl_kodsb{i}")))
                            break;
                        taskDetail.PriceException.Fees.Add(new AmendmentPriceException.Types.FeesItem
                        {
                            FeeId = taskData.GetValueOrDefault($"ukol_overeni_ic_popl_kodsb{i}"),
                            TariffSum = taskData.GetNInteger($"ukol_overeni_ic_popl_sazeb{i}"),
                            FinalSum = taskData.GetNInteger($"ukol_overeni_ic_popl_vysl{i}"),
                            DiscountPercentage = taskData.GetNInteger($"ukol_overeni_ic_popl_sleva_perc{i}")
                        });
                    }
                    break;

                case 6:
                    taskDetail.Signing = new()
                    {
                        FormId = taskData.GetValueOrDefault("ukol_podpis_dokument_form_id") ?? "",
                        Expiration = taskData.GetDate("ukol_podpis_lhuta_do"),
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

    private static int? getStateIndicator(IReadOnlyDictionary<string, string> taskData)
    {
        if (taskData.GetNInteger("ukol_stornovano") == 1)
        {
            return 2;
        }
        else if (taskData.GetNInteger("ukol_stav_poz") == 30)
        {
            return 3;
        }
        else
        {
            return 1;
        }
    }

    private static int getPhaseTypeId(int taskTypeId, IReadOnlyDictionary<string, string> taskData)
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

    private static string getSignatureType(int taskTypeId, IReadOnlyDictionary<string, string> taskData)
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

    private static string getStateName(IReadOnlyDictionary<string, string> taskData)
    {
        if (taskData.GetBoolean("ukol_stornovano"))
        {
            return "ZRUŠENO";
        }
        else if (taskData.GetInteger("ukol_stav_poz") == 30)
        {
            return "DOKONČENO";
        }
        else
        {
            return taskData.GetValueOrDefault("ukol_proces_oznacenie_noby") ?? "";
        }
    }

    private static int[] _allowedConsultationTypes = new[] { 1, 7 };
}