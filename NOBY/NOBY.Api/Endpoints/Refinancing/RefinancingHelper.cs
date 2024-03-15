using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.SalesArrangementService.Contracts;
using _SaContract = DomainServices.SalesArrangementService.Contracts;
namespace NOBY.Api.Endpoints.Refinancing;

public static class RefinancingHelper
{
    public static string GetRefinancingTypeText(List<EaCodesMainResponse.Types.EaCodesMainItem> eaCodesMain, ProcessTask process, List<GenericCodebookResponse.Types.GenericCodebookItem> refinancingTypes)
    {
        var text = eaCodesMain.Find(e => e.Id == process.RefinancingProcess.RefinancingDocumentEACode)?.Name;

        if (string.IsNullOrWhiteSpace(text))
        {
            text = process.RefinancingProcess.RefinancingType switch
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

    public static RefinancingTypes GetRefinancingType(ProcessTask process)
    {
        return process.ProcessTypeId switch
        {
            3 when process.RefinancingProcess.RefinancingType == 1 => RefinancingTypes.Retence,  // Retence
            3 when process.RefinancingProcess.RefinancingType == 2 => RefinancingTypes.Refixace, // Refixace
            _ => throw new NotSupportedException()
        };
    }

    public static DateTime? GetFixedRateValidFrom(DateTime? fixedRateValidTo, int? fixedRatePeriod)
    {
        return fixedRatePeriod is not null && fixedRateValidTo is not null
            ? ((DateTime)fixedRateValidTo).AddMonths(-fixedRatePeriod.Value)
            : (DateTime?)default;
    }

    public static RefinancingStates GetRefinancingState(_SaContract.SalesArrangement? sa, ProcessTask process)
    {
        if (!process.Cancelled && process.StateIdSB != 30 && sa?.Retention?.ManagedByRC2 != true && process.ProcessPhaseId == 1 && process.ProcessId == sa?.TaskProcessId)
        {
            return RefinancingStates.RozpracovanoVNoby; // 1
        }
        else if (!process.Cancelled && process.StateIdSB != 30 && sa?.Retention?.ManagedByRC2 != true && process.ProcessPhaseId == 1 && process.ProcessId != sa?.TaskProcessId)
        {
            return RefinancingStates.RozpracovanoVSB;  // 2
        }
        else if (!process.Cancelled && process.StateIdSB != 30 && sa?.Retention?.ManagedByRC2 != true && process.ProcessPhaseId == 3)
        {
            return RefinancingStates.Podepisovani; // 3
        }
        else if (!process.Cancelled && process.StateIdSB == 30)
        {
            return RefinancingStates.Dokonceno; // 4 
        }
        else if (!process.Cancelled && process.StateIdSB != 30 && sa?.Retention?.ManagedByRC2 == true)
        {
            return RefinancingStates.PredanoRC2; // 5
        }
        else if (process.Cancelled)
        {
            return RefinancingStates.Zruseno; // 6
        }
        else
        {
            throw new ArgumentException("Unsupported state (RefinancingStates)"); // Unknow 
        }
    }
}
