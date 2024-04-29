namespace ExternalServices.EasSimulationHT.Dto;

public sealed class MortgageExtraPaymentResult
{
    public bool IsExtraPaymentComplete { get; set; }
    public decimal ExtraPaymentAmount { get; set; }
    public decimal FeeAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal OtherUnpaidFees { get; set; }
    public decimal InterestOnLate { get; set; }
    public decimal InterestCovid { get; set; }
    public bool IsLoanOverdue { get; set; }
    public bool IsPaymentReduced { get; set; }
    public DateTime NewMaturityDate { get; set; }
    public decimal NewPaymentAmount { get; set; }
    public int FeeTypeId { get; set; }
    public decimal FeeCalculationBase { get; set; }
    public decimal FeeAmountContracted { get; set; }
    public DateTime FixedRateSanctionFreePeriodFrom { get; set; }
    public DateTime FixedRateSanctionFreePeriodTo { get; set; }
    public DateTime AnnualSanctionFreePeriodFrom { get; set; }
    public DateTime AnnualSanctionFreePeriodTo { get; set; }
    public decimal SanctionFreeAmount { get; set; }
}
