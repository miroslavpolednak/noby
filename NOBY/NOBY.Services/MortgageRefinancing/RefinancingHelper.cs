using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Services.MortgageRefinancing;

public static class RefinancingHelper
{
    public static TResponse UpdateBaseResponseModel<TResponse>(this GetRefinancingDataResult result, TResponse response)
        where TResponse : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
    {
        response.RefinancingStateId = result.RefinancingState;
        response.SalesArrangementId = result.SalesArrangement?.SalesArrangementId;
        response.IsReadOnly = result.RefinancingState != RefinancingStates.RozpracovanoVNoby && result.Process is not null;
        response.IsPriceExceptionActive = result.ActivePriceException is not null && !result.IsActivePriceExceptionCompleted;
        response.Tasks = result.Tasks;

        return response;
    }

    public static string GetRefinancingTypeText(List<EaCodesMainResponse.Types.EaCodesMainItem> eaCodesMain, ProcessTask process, List<GenericCodebookResponse.Types.GenericCodebookItem> refinancingTypes)
    {
        int? eacode = process.AmendmentsCase switch
        {
            ProcessTask.AmendmentsOneofCase.MortgageRetention => process.MortgageRetention.DocumentEACode,
            ProcessTask.AmendmentsOneofCase.MortgageRefixation => process.MortgageRefixation.DocumentEACode,
            ProcessTask.AmendmentsOneofCase.MortgageExtraPayment => process.MortgageExtraPayment.DocumentEACode,
            ProcessTask.AmendmentsOneofCase.MortgageLegalNotice => process.MortgageLegalNotice.DocumentEACode,
            _ => null
        };
        var text = eaCodesMain.Find(e => e.Id == eacode)?.Name;

        if (string.IsNullOrWhiteSpace(text))
        {
            text = refinancingTypes.FirstOrDefault(r => r.Id == (int)GetRefinancingType(process))?.Name ?? "";
        }
        return text;
    }

    public static bool IsAnotherSalesArrangementInProgress(List<DomainServices.SalesArrangementService.Contracts.SalesArrangement> saList)
    {
        return saList.Any(t => _activeSalesArrangementStates.Contains(t.State) 
            && (SalesArrangementTypes)t.SalesArrangementTypeId is (SalesArrangementTypes.GeneralChange
                or SalesArrangementTypes.HUBN
                or SalesArrangementTypes.CustomerChange
				or SalesArrangementTypes.Drawing
				or SalesArrangementTypes.CustomerChange3602A
                or SalesArrangementTypes.CustomerChange3602B
                or SalesArrangementTypes.CustomerChange3602C));
    }

    public static RefinancingTypes GetRefinancingType(ProcessTask process)
    {
        return process.AmendmentsCase switch
        {
            ProcessTask.AmendmentsOneofCase.MortgageRetention => RefinancingTypes.MortgageRetention,
            ProcessTask.AmendmentsOneofCase.MortgageRefixation => RefinancingTypes.MortgageRefixation,
            ProcessTask.AmendmentsOneofCase.MortgageExtraPayment => RefinancingTypes.MortgageExtraPayment,
            ProcessTask.AmendmentsOneofCase.MortgageLegalNotice => RefinancingTypes.MortgageLegalNotice,
            _ => RefinancingTypes.Unknown
        };
    }

    public static DateTime? GetFixedRateValidFrom(in DateTime? fixedRateValidTo, in int? fixedRatePeriod)
    {
        return fixedRatePeriod is not null && fixedRateValidTo is not null
            ? ((DateTime)fixedRateValidTo).AddMonths(-fixedRatePeriod.Value)
            : (DateTime?)default;
    }

    public static RefinancingStates GetRefinancingState(in SalesArrangementStates salesArrangementState, in bool managedByRC2, ProcessTask process)
    {
        if (salesArrangementState != SalesArrangementStates.Unknown && !managedByRC2)
        {
            return salesArrangementState switch
            {
                SalesArrangementStates.InSigning => RefinancingStates.PodpisNOBY,
                SalesArrangementStates.Finished => RefinancingStates.Dokonceno,
                SalesArrangementStates.Cancelled => RefinancingStates.Zruseno,
                _ => RefinancingStates.RozpracovanoVNoby
            };
        }
        else if (salesArrangementState == SalesArrangementStates.Unknown || (salesArrangementState != SalesArrangementStates.Unknown && managedByRC2))
        {
            if (!process.Cancelled && process.StateIdSB != 30 && process.ProcessPhaseId == 2 && salesArrangementState == SalesArrangementStates.Unknown)
            {
                return RefinancingStates.RozpracovanoVSB;
            }
            else if (!process.Cancelled && process.StateIdSB != 30 && process.ProcessPhaseId == 2 && salesArrangementState != SalesArrangementStates.Unknown)
            {
                return RefinancingStates.PredanoRC2;
            }
            else if (!process.Cancelled && process.StateIdSB == 30)
            {
                return RefinancingStates.Dokonceno;
            }
            else if (process.Cancelled && process.StateIdSB == 30)
            {
                return RefinancingStates.Zruseno;
            }
            else if (!process.Cancelled && process.StateIdSB != 30 && process.ProcessPhaseId == 3)
            {
                return RefinancingStates.PodpisSB;
            }
            else
            {
                throw new ArgumentException("Unsupported RefinancingStates");
            }
        }
        else
        {
            throw new ArgumentException("Unsupported RefinancingStates");
        }
    }

    private static readonly int[] _activeSalesArrangementStates = 
    [
        (int)SalesArrangementStates.InProgress,
        //(int)SalesArrangementStates.NewArrangement, tmp HACH-11963
        (int)SalesArrangementStates.InSigning,
        (int)SalesArrangementStates.ToSend,
        (int)SalesArrangementStates.RC2,
    ];
}
