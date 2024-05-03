﻿using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Services.MortgageRefinancing;

public static class RefinancingHelper
{
    public static TResponse UpdateBaseResponseModel<TResponse>(this GetRefinancingDataResult result, TResponse response)
        where TResponse : NOBY.Dto.Refinancing.BaseRefinancingDetailResponse
    {
        response.RefinancingStateId = result.RefinancingState;
        response.SalesArrangementId = result.SalesArrangement?.SalesArrangementId;
        response.IsReadOnly = result.RefinancingState != RefinancingStates.RozpracovanoVNoby;
        response.IsPriceExceptionActive = result.ActivePriceException is not null;
        response.Tasks = result.Tasks;

        return response;
    }

    public static string GetRefinancingTypeText(List<EaCodesMainResponse.Types.EaCodesMainItem> eaCodesMain, ProcessTask process, List<GenericCodebookResponse.Types.GenericCodebookItem> refinancingTypes)
    {
        int? eacode = process.AmendmentsCase switch
        {
            ProcessTask.AmendmentsOneofCase.MortgageRetention => process.MortgageRetention.DocumentEACode,
            ProcessTask.AmendmentsOneofCase.MortgageRefixation => process.MortgageRetention.DocumentEACode,
            ProcessTask.AmendmentsOneofCase.MortgageExtraPayment => process.MortgageRetention.DocumentEACode,
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
        var activeSas = saList.Where(s => (SalesArrangementStates)s.State is SalesArrangementStates.InProgress
                                                                  or SalesArrangementStates.InApproval
                                                                  or SalesArrangementStates.NewArrangement
                                                                  or SalesArrangementStates.InSigning
                                                                  or SalesArrangementStates.ToSend
                                                                  or SalesArrangementStates.RC2);

        return activeSas.Any(s => (SalesArrangementTypes)s.SalesArrangementTypeId is SalesArrangementTypes.GeneralChange
                                                           or SalesArrangementTypes.HUBN
                                                           or SalesArrangementTypes.CustomerChange
                                                           or SalesArrangementTypes.CustomerChange3602A);
    }

    public static RefinancingTypes GetRefinancingType(ProcessTask process)
    {
        return process.AmendmentsCase switch
        {
            ProcessTask.AmendmentsOneofCase.MortgageRetention => RefinancingTypes.MortgageRetention,
            ProcessTask.AmendmentsOneofCase.MortgageRefixation => RefinancingTypes.MortgageRefixation,
            ProcessTask.AmendmentsOneofCase.MortgageExtraPayment => RefinancingTypes.MortgageExtraPayment,
            _ => RefinancingTypes.Unknown
        };
    }

    public static DateTime? GetFixedRateValidFrom(in DateTime? fixedRateValidTo, in int? fixedRatePeriod)
    {
        return fixedRatePeriod is not null && fixedRateValidTo is not null
            ? ((DateTime)fixedRateValidTo).AddMonths(-fixedRatePeriod.Value)
            : (DateTime?)default;
    }

    public static RefinancingStates GetRefinancingState(in SalesArrangementStates salesArrangementState)
        => salesArrangementState switch
        {
            SalesArrangementStates.InSigning => RefinancingStates.PodpisNOBY,
            SalesArrangementStates.Finished => RefinancingStates.Dokonceno,
            SalesArrangementStates.Cancelled => RefinancingStates.Zruseno,
            _ => RefinancingStates.RozpracovanoVNoby
        };

    public static RefinancingStates GetRefinancingState(in bool managedByRC2, in long? processId, ProcessTask process)
    {
        if (!process.Cancelled && process.StateIdSB != 30 && managedByRC2 != true && process.ProcessPhaseId == 1 && process.ProcessId == processId)
        {
            return RefinancingStates.RozpracovanoVNoby; // 1
        }
        else if (!process.Cancelled && process.StateIdSB != 30 && managedByRC2 != true && process.ProcessPhaseId == 1 && process.ProcessId != processId)
        {
            return RefinancingStates.RozpracovanoVSB;  // 2
        }
        else if (!process.Cancelled && process.StateIdSB != 30 && managedByRC2 != true && process.ProcessPhaseId == 3)
        {
            return RefinancingStates.PodpisNOBY; // 3
        }
        else if (!process.Cancelled && process.StateIdSB == 30)
        {
            return RefinancingStates.Dokonceno; // 4 
        }
        else if (!process.Cancelled && process.StateIdSB != 30 && managedByRC2 == true)
        {
            return RefinancingStates.PredanoRC2; // 5
        }
        else if (process.Cancelled)
        {
            return RefinancingStates.Zruseno; // 6
        }
        else
        {
            throw new ArgumentException("Unsupported RefinancingStates");
        }
    }
}
