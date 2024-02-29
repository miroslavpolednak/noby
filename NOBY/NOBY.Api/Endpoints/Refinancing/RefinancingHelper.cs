using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.SalesArrangementService.Contracts;
using _SaContract = DomainServices.SalesArrangementService.Contracts;
namespace NOBY.Api.Endpoints.Refinancing;

public static class RefinancingHelper
{
    public static string GetRefinancingTypeText(List<EaCodesMainResponse.Types.EaCodesMainItem> eaCodesMain, ProcessTask process, List<GenericCodebookResponse.Types.GenericCodebookItem> refinancingTypes)
    {
        var text = eaCodesMain.Find(e => e.Id == process.RetentionProcess.RefinancingDocumentEACode)?.Name;

        if (string.IsNullOrWhiteSpace(text))
        {
            text = process.RetentionProcess.RefinancingType switch
            {
                1 => refinancingTypes.Single(r => r.Id == (int)RefinancingTypes.Retence).Name,
                2 => refinancingTypes.Single(r => r.Id == (int)RefinancingTypes.Refixace).Name,
                3 => refinancingTypes.Single(r => r.Id == (int)RefinancingTypes.MimoradnaSplatka).Name,
                _ => string.Empty
            };
        }
        return text;
    }

    public static bool IsAnotherSalesArrangementInProgress(GetSalesArrangementListResponse saList)
    {
        var activeSas = saList.SalesArrangements.Where(s => s.State is (int)SalesArrangementStates.InProgress
                                                                  or (int)SalesArrangementStates.InApproval
                                                                  or (int)SalesArrangementStates.NewArrangement
                                                                  or (int)SalesArrangementStates.InSigning
                                                                  or (int)SalesArrangementStates.ToSend
                                                                  or (int)SalesArrangementStates.RC2);

        return activeSas.Any(s => s.SalesArrangementTypeId is (int)SalesArrangementTypes.GeneralChange
                                                           or (int)SalesArrangementTypes.HUBN
                                                           or (int)SalesArrangementTypes.CustomerChange
                                                           or (int)SalesArrangementTypes.CustomerChange3602A);
    }

    public static int GetRefinancingType(ProcessTask process)
    {
        return process.ProcessTypeId switch
        {
            3 when process.RetentionProcess.RefinancingType == 1 => 1, // Retence
            3 when process.RetentionProcess.RefinancingType == 2 => 2, // Refixace
            _ => 0
        };
    }

    public static DateTime? GetFixedRateValidFrom(DateTime? fixedRateValidTo, int? fixedRatePeriod)
    {
        return fixedRatePeriod is not null && fixedRateValidTo is not null
            ? ((DateTime)fixedRateValidTo).AddMonths(-fixedRatePeriod.Value)
            : (DateTime?)default;
    }

    public static int GetRefinancingState(_SaContract.SalesArrangement? sa, ProcessTask process)
    {
        if (!process.Cancelled && process.StateIdSB != 30 && sa?.Retention?.ManagedByRC2 == false && process.ProcessPhaseId == 1 && process.ProcessId == sa.TaskProcessId)
        {
            return (int)RefinancingStates.RozpracovanoVNoby; // 1
        }
        else if (!process.Cancelled && process.StateIdSB != 30 && sa?.Retention?.ManagedByRC2 == false && process.ProcessPhaseId == 1 && process.ProcessId != sa.TaskProcessId)
        {
            return (int)RefinancingStates.RozpracovanoVSB;  // 2
        }
        else if (!process.Cancelled && process.StateIdSB != 30 && sa?.Retention?.ManagedByRC2 == false && process.ProcessPhaseId == 3)
        {
            return (int)RefinancingStates.Podepisovani; // 3
        }
        else if (!process.Cancelled && process.StateIdSB == 30)
        {
            return (int)RefinancingStates.Dokonceno; // 4 
        }
        else if (!process.Cancelled && process.StateIdSB != 30 && sa?.Retention?.ManagedByRC2 == true)
        {
            return (int)RefinancingStates.PredanoRC2; // 5
        }
        else if (process.Cancelled)
        {
            return (int)RefinancingStates.Zruseno; // 6
        }
        else
        {
            return 0; // Unknow 
        }
    }
}
