using DomainServices.CaseService.Contracts;
using FastEnumUtility;

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
            PerformerLogin = taskData.GetValueOrDefault("ukol_op_zpracovatel") ?? "",
            ProcessTypeId = taskData.GetInteger("ukol_top_proces_typ_noby"),
        };

        task.PhaseTypeId = task.TaskTypeId switch
        {
            1 => taskData.GetInteger("ukol_dozadani_stav"),
            2 => taskData.GetInteger("ukol_overeni_ic_stav"),
            6 => taskData.GetInteger("ukol_podpis_stav"),
            3 or 4 or 7 or 8 => 1,
            9 => taskData.GetInteger("ukol_faze_rt_procesu"),
            _ => throw new ArgumentOutOfRangeException(nameof(task.PhaseTypeId), "PhaseTypeId can not be set")
        };

        task.SignatureTypeId = (task.TaskTypeId, taskData.GetNInteger("ukol_podpis_dokument_metoda")) switch
        {
            (6, 1) => SignatureTypes.Paper.ToByte(),
            (6, 2) => SignatureTypes.Electronic.ToByte(),
            (6, 3) => SignatureTypes.Paper.ToByte(),
            _ => null
        };

        task.DecisionId = task.TaskTypeId == 2 ? taskData.GetNInteger("ukol_overeni_ic_zpusob_reseni") : null;

        return task;
    }

    public static ProcessTask ToProcessTask(this IReadOnlyDictionary<string, string> taskData)
    {
        var taskType = taskData.GetInteger("ukol_proces_typ_noby");
        var taskSubType = taskData.GetNInteger("ukol_retence_druh");

        var result = new ProcessTask
        {
            ProcessIdSb = taskData.GetInteger("ukol_id"),
            ProcessId = taskData.GetLong("ukol_sada"),
            CreatedOn = taskData.GetDate("ukol_dat_start_proces"),
            ProcessTypeId = taskType,
            ProcessNameLong = taskData.GetValueOrDefault("ukol_proces_nazev_noby") ?? "",
            StateName = getStateName(taskData),
            ProcessPhaseId = taskData.GetInteger("ukol_proces_typ_noby") switch
            {
                1 => taskData.GetInteger("ukol_faze_uv_procesu"),
                2 => taskData.GetInteger("ukol_faze_zm_procesu"),
                3 => taskData.GetInteger("ukol_faze_rt_procesu"),
                4 => 1,
                -1 => -1,
                _ => throw new CisArgumentException(0, taskType.ToString(CultureInfo.InvariantCulture), "ProcessPhaseId")
            },
            StateIndicator = taskType switch
            {
                1 or 2 or 3 or 4 => getStateIndicator(taskData),
                _ => null
            },
            StateIdSB = taskData.GetInteger("ukol_stav_poz"),
            Cancelled = taskData.GetBoolean("ukol_stornovano")
        };

        
        switch (taskType)
        {
            case 3 when taskSubType == 1:
                result.RefinancingType = (int)RefinancingTypes.MortgageRetention;
                result.MortgageRetention = GetRetentionProcess(taskData);
                break;

            case 3 when taskSubType == 2:
                result.RefinancingType = (int)RefinancingTypes.MortgageRefixation;
                result.MortgageRefixation = GetRefixationProcess(taskData);
                break;

            case 3 when taskSubType == 3:
                result.RefinancingType = (int)RefinancingTypes.MortgageLegalNotice;
                result.MortgageLegalNotice = GetLegalNoticeProcess(taskData);
                break;

            case 4:
                result.RefinancingType = (int)RefinancingTypes.MortgageExtraPayment;
                result.MortgageExtraPayment = GetExtraPaymentProcess(taskData);
                break;
        }

        return result;
    }

    private static Contracts.ProcessTask.Types.TaskAmendmentMortgageLegalNotice GetLegalNoticeProcess(IReadOnlyDictionary<string, string> taskData)
        => new()
        {
            LoanInterestRateProvided = taskData.GetNDecimal("ukol_retence_sazba_vysl"),
            LoanPaymentAmountFinal = taskData.GetNDecimal("ukol_retence_splatka_vysl"),
            FixedRatePeriod = taskData.GetNInteger("ukol_retence_perioda_fixace"),
            DocumentId = taskData.GetValueOrDefault("ukol_retence_dokument_ea_cis") ?? "",
            DocumentEACode = taskData.GetNInteger("ukol_retence_dokument_ea_kod")
        };

    private static Contracts.ProcessTask.Types.TaskAmendmentMortgageExtraPayment GetExtraPaymentProcess(IReadOnlyDictionary<string, string> taskData)
    {
        var result = new Contracts.ProcessTask.Types.TaskAmendmentMortgageExtraPayment()
        {
            ExtraPaymentDate = taskData.GetDate("ukol_mspl_dat_spl"),
            ExtraPaymentAmount = taskData.GetNDecimal("ukol_mspl_suma"),
            ExtraPaymentAmountIncludingFee = taskData.GetNDecimal("ukol_mspl_suma_celkem"),
            IsFinalExtraPayment = taskData.GetBool("ukol_mspl_typ"),
            DocumentId = taskData.GetValueOrDefault("ukol_retence_dokument_ea_cis") ?? "",
            DocumentEACode = taskData.GetNInteger("ukol_retence_dokument_ea_kod"),
            PaymentState = taskData.GetInteger("ukol_mspl_stav_zauct_noby"),
        };

        var docs = taskData["ukol_mspl_souhlas_ea_cis"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var docsEa = taskData["ukol_mspl_souhlas_ea_kod"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (docs is not null && docsEa is not null)
        {
            for (int i = 0; i < docs.Length; i++)
            {
                result.ExtraPaymentAgreements.Add(new ProcessTask.Types.ExtraPaymentAgreement
                {
                    AgreementDocumentId = docs[i],
                    AgreementEACode = Convert.ToInt32(docsEa[i], CultureInfo.InvariantCulture)
                });
            }
        }

        return result;
    }
    
    private static Contracts.ProcessTask.Types.TaskAmendmentMortgageRetention GetRetentionProcess(IReadOnlyDictionary<string, string> taskData)
        => new()
        {
            InterestRateValidFrom = taskData.GetDate("ukol_retence_sazba_dat_od"),
            LoanInterestRate = taskData.GetNDecimal("ukol_retence_sazba_kalk"),
            LoanInterestRateProvided = taskData.GetNDecimal("ukol_retence_sazba_vysl"),
            LoanPaymentAmount = taskData.GetNInteger("ukol_retence_splatka_kalk"),
            LoanPaymentAmountFinal = taskData.GetNInteger("ukol_retence_splatka_vysl"),
            FeeSum = taskData.GetNInteger("ukol_retence_popl_kalk"),
            FeeFinalSum = taskData.GetNInteger("ukol_retence_popl_vysl"),
            DocumentId = taskData.GetValueOrDefault("ukol_retence_dokument_ea_cis") ?? "",
            DocumentEACode = taskData.GetNInteger("ukol_retence_dokument_ea_kod"),
            EffectiveDate = taskData.GetDate("ukol_retence_dat_ucinnost")
        };

    private static Contracts.ProcessTask.Types.TaskAmendmentMortgageRefixation GetRefixationProcess(IReadOnlyDictionary<string, string> taskData)
        => new()
        {
            LoanInterestRate = taskData.GetNDecimal("ukol_retence_sazba_kalk"),
            LoanInterestRateProvided = taskData.GetNDecimal("ukol_retence_sazba_vysl"),
            LoanPaymentAmount = taskData.GetNInteger("ukol_retence_splatka_kalk"),
            LoanPaymentAmountFinal = taskData.GetNInteger("ukol_retence_splatka_vysl"),
            FixedRatePeriod = taskData.GetInteger("ukol_retence_perioda_fixace"),
            DocumentId = taskData.GetValueOrDefault("ukol_retence_dokument_ea_cis") ?? "",
            DocumentEACode = taskData.GetNInteger("ukol_retence_dokument_ea_kod"),
            EffectiveDate = taskData.GetDate("ukol_retence_dat_ucinnost")
        };

    public static TaskDetailItem ToTaskDetail(this IReadOnlyDictionary<string, string> taskData)
    {
        var taskDetail = new TaskDetailItem
        {
            ProcessNameLong = taskData.GetValueOrDefault("ukol_top_proces_nazev_noby") ?? "",
            TaskDocumentIds = { (taskData.GetValueOrDefault("wfl_refobj_dokumenty") ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries) }
        };

        if (int.TryParse(taskData.GetValueOrDefault("ukol_typ_noby"), out int taskType))
        {
            switch (taskType)
            {
                case 1:
                    taskDetail.Request = new()
                    {
                        OrderId = taskData.GetValueOrDefault("ukol_dozadani_typ") == "5" ? taskData.GetNInteger("ukol_dozadani_order_id") : null
                    };

                    switch (taskData.GetValueOrDefault("ukol_dozadani_prijemce_typ"))
                    {
                        case "0": 
                            taskDetail.Request.SentToCustomer = false;
                            break;
                        case "1" or "2" or "3":
							taskDetail.Request.SentToCustomer = true;
							break;
                    }
                    break;

                // price exception
                case 2:
                    taskDetail.PriceException = new()
                    {
                        Expiration = taskData.GetDate("ukol_overeni_ic_sazba_dat_do"),
                        LoanInterestRate = new()
                        {
                            LoanInterestRate = taskData.GetDecimal("ukol_overeni_ic_sazba_nabid"),
                            LoanInterestRateProvided = taskData.GetDecimal("ukol_overeni_ic_sazba_vysled"),
                            LoanInterestRateAnnouncedType = taskData.GetInteger("ukol_overeni_ic_sazba_typ"),
                            LoanInterestRateDiscount = taskData.GetDecimal("ukol_overeni_ic_sazba_sleva")
                        }
                    };
                    for (int i = 1; i < 20; i++)
                    {
                        if (string.IsNullOrEmpty(taskData.GetValueOrDefault($"ukol_overeni_ic_popl_kodsb{i}")) ||
                            taskData.GetValueOrDefault($"ukol_overeni_ic_popl_kodsb{i}") == "0")
                        {
                            break;
                        }

                        taskDetail.PriceException.Fees.Add(new PriceExceptionFeesItem
                        {
                            FeeId = taskData.GetInteger($"ukol_overeni_ic_popl_kodsb{i}"),
                            TariffSum = taskData.GetDecimal($"ukol_overeni_ic_popl_sazeb{i}"),
                            FinalSum = taskData.GetDecimal($"ukol_overeni_ic_popl_vysl{i}"),
                            DiscountPercentage = taskData.GetDecimal($"ukol_overeni_ic_popl_sleva_perc{i}")
                        });
                    }
                    break;

                case 4:
                    taskDetail.RealEstateValuation = new()
                    {
                        OrderId = taskData.GetInteger("ukol_odhad_order_id"),
                        DocumentInfoPrice = taskData.GetValueOrDefault("ukol_odhad_ea_docs_infocena"),
                        DocumentRecommendationForClient = taskData.GetValueOrDefault("ukol_odhad_ea_docs_doporuceni"),
                        OnlineValuation = taskData.GetValueOrDefault("ukol_odhad_valuation_type") == "OCEN_LUX"
                    };
                    break;

                case 6:
                    taskDetail.Signing = new()
                    {
                        FormId = taskData.GetValueOrDefault("ukol_podpis_dokument_form_id") ?? "",
                        Expiration = taskData.GetDate("ukol_podpis_lhuta_do"),
                        DocumentForSigning = taskData.GetValueOrDefault("ukol_podpis_dokument_ep_id") ?? "",
                        DocumentForSigningType = taskData.GetValueOrDefault("ukol_podpis_dokument_ep_typ") ?? ""
                    };

                    if (int.TryParse(taskData.GetValueOrDefault("ukol_podpis_dokument_typ"), out int eACodeMain))
                    {
                        taskDetail.Signing.EACodeMain = eACodeMain;
                    }

                    if (!string.IsNullOrEmpty(taskData.GetValueOrDefault("ukol_podpis_prilohy_ep_id")))
                    {
                        taskDetail.Signing.ProposalForEntry.AddRange(taskData.GetValueOrDefault("ukol_podpis_prilohy_ep_id")!.Split(',', StringSplitOptions.RemoveEmptyEntries));
                    }
                    break;

                case 3 when _allowedConsultationTypes.Contains(taskData.GetNInteger("ukol_konzultace_oblast").GetValueOrDefault()):
                    taskDetail.ConsultationData = new()
                    {
                        OrderId = taskData.GetNInteger("ukol_konzultace_order_id"),
                        TaskSubtypeId = taskData.GetInteger("ukol_konzultace_oblast")
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
            TaskIdSb = workflowTask.TaskIdSb,
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