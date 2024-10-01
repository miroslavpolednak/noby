using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Services.MortgageRefinancing;

public static class RefinancingHelper
{
    public static TResponse UpdateBaseResponseModel<TResponse>(this GetRefinancingDataResult result, TResponse response)
        where TResponse : NOBY.ApiContracts.IRefinancingDataResult
    {
        response.RefinancingStateId = result.RefinancingState;
        response.SalesArrangementId = result.SalesArrangement?.SalesArrangementId;
        response.IsReadOnly = result.RefinancingState != EnumRefinancingStates.RozpracovanoVNoby && result.Process is not null;
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
        return saList.Any(t => t.IsInState(_activeSalesArrangementStates) 
            && (SalesArrangementTypes)t.SalesArrangementTypeId is (SalesArrangementTypes.GeneralChange
                or SalesArrangementTypes.HUBN
                or SalesArrangementTypes.CustomerChange
				or SalesArrangementTypes.Drawing
				or SalesArrangementTypes.CustomerChange3602A
                or SalesArrangementTypes.CustomerChange3602B
                or SalesArrangementTypes.CustomerChange3602C));
    }

    public static EnumRefinancingTypes GetRefinancingType(ProcessTask process)
    {
        return process.AmendmentsCase switch
        {
            ProcessTask.AmendmentsOneofCase.MortgageRetention => EnumRefinancingTypes.MortgageRetention,
            ProcessTask.AmendmentsOneofCase.MortgageRefixation => EnumRefinancingTypes.MortgageRefixation,
            ProcessTask.AmendmentsOneofCase.MortgageExtraPayment => EnumRefinancingTypes.MortgageExtraPayment,
            ProcessTask.AmendmentsOneofCase.MortgageLegalNotice => EnumRefinancingTypes.MortgageLegalNotice,
            _ => EnumRefinancingTypes.Unknown
        };
    }

    public static DateOnly? GetFixedRateValidFrom(in DateOnly? fixedRateValidTo, in int? fixedRatePeriod)
    {
        return fixedRatePeriod is not null && fixedRateValidTo is not null
            ? ((DateOnly)fixedRateValidTo).AddMonths(-fixedRatePeriod.Value)
            : default;
    }

    public static EnumRefinancingStates GetRefinancingState(in EnumSalesArrangementStates salesArrangementState, in bool managedByRC2, ProcessTask process)
    {
        if (salesArrangementState != EnumSalesArrangementStates.Unknown && !managedByRC2)
        {
            return salesArrangementState switch
            {
                EnumSalesArrangementStates.InSigning => EnumRefinancingStates.PodpisNOBY,
                EnumSalesArrangementStates.Finished => EnumRefinancingStates.Dokonceno,
                EnumSalesArrangementStates.Cancelled => EnumRefinancingStates.Zruseno,
                _ => EnumRefinancingStates.RozpracovanoVNoby
            };
        }
        else if (salesArrangementState == EnumSalesArrangementStates.Unknown || (salesArrangementState != EnumSalesArrangementStates.Unknown && managedByRC2))
        {
            if (!process.Cancelled && process.StateIdSB != 30 && process.ProcessPhaseId == 2 && salesArrangementState == EnumSalesArrangementStates.Unknown)
            {
                return EnumRefinancingStates.RozpracovanoVSB;
            }
            else if (!process.Cancelled && process.StateIdSB != 30 && process.ProcessPhaseId == 2 && salesArrangementState != EnumSalesArrangementStates.Unknown)
            {
                return EnumRefinancingStates.PredanoRC2;
            }
            else if (!process.Cancelled && process.StateIdSB == 30)
            {
                return EnumRefinancingStates.Dokonceno;
            }
            else if (process.Cancelled && process.StateIdSB == 30)
            {
                return EnumRefinancingStates.Zruseno;
            }
            else if (!process.Cancelled && process.StateIdSB != 30 && process.ProcessPhaseId == 3)
            {
                return EnumRefinancingStates.PodpisSB;
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

    private static readonly EnumSalesArrangementStates[] _activeSalesArrangementStates = 
    [
        EnumSalesArrangementStates.InProgress,
        EnumSalesArrangementStates.InSigning,
        EnumSalesArrangementStates.ToSend,
        EnumSalesArrangementStates.RC2,
    ];
}
