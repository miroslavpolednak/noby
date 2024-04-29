namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

internal static class MortgageExtraPaymentExtensions
{
    public static Dto.Refinancing.ExtraPaymentSimulationResult ToDto(this DomainServices.OfferService.Contracts.MortgageExtraPaymentSimulationResults result, DateTime createdOn, decimal? feeAmountDiscounted)
        => new()
        {
            ExtraPaymentAmount = result.ExtraPaymentAmount,
            FeeAmount = result.FeeAmount,
            InterestAmount = result.InterestAmount,
            InterestCovid = result.InterestCovid,
            InterestOnLate = result.InterestOnLate,
            IsExtraPaymentComplete = result.IsExtraPaymentFullyRepaid,
            NewMaturityDate = result.NewMaturityDate,
            IsLoanOverdue = result.IsLoanOverdue,
            IsPaymentReduced = result.IsPaymentReduced,
            NewPaymentAmount = result.NewPaymentAmount,
            OtherUnpaidFees = result.OtherUnpaidFees,
            PrincipalAmount = result.PrincipalAmount,
            FeeAmountContracted = result.FeeAmountContracted,
            SanctionFreeAmount = result.SanctionFreeAmount,
            FixedRateSanctionFreePeriodFrom = result.FixedRateSanctionFreePeriodFrom,
            FixedRateSanctionFreePeriodTo = result.FixedRateSanctionFreePeriodTo,
            FeeCalculationBase = result.FeeCalculationBase,
            FeeTypeId = result.FeeTypeId,
            AnnualSanctionFreePeriodFrom = result.AnnualSanctionFreePeriodFrom,
            AnnualSanctionFreePeriodTo = result.AnnualSanctionFreePeriodTo,
            CreatedOn = createdOn,

            FeeAmountTotal = result.FeeAmount - feeAmountDiscounted.GetValueOrDefault(),
            ExtraPaymentAmountTotal = result.ExtraPaymentAmount + result.FeeAmount - feeAmountDiscounted.GetValueOrDefault()
        };
}
