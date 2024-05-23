namespace DomainServices.OfferService.Api.Database.DocumentDataEntities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

internal sealed class MortgageExtraPaymentData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public SimulationInputsData SimulationInputs { get; set; }
    public SimulationOutputsData SimulationOutputs { get; set; }
    public BasicParametersData BasicParameters { get; set; }

    public sealed class SimulationInputsData
    {
        public DateTime ExtraPaymentDate { get; set; }
        public decimal? ExtraPaymentAmount { get; set; }
        public int ExtraPaymentReasonId { get; set; }
        public bool IsExtraPaymentFullyRepaid { get; set; }
    }

    public sealed class SimulationOutputsData
    {
        public bool IsExtraPaymentFullyRepaid { get; set; }
        public decimal ExtraPaymentAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal OtherUnpaidFees { get; set; }
        public decimal InterestOnLate { get; set; }
        public decimal InterestCovid { get; set; }
        public bool IsLoanOverdue { get; set; }
        public bool IsInstallmentReduced { get; set; }
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
		public int ExtraPaymentReasonId { get; set; }
	}

    public sealed class BasicParametersData
    {
        public decimal? FeeAmountDiscount { get; set; }
    }
}
