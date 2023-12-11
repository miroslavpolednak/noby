namespace DomainServices.OfferService.Api.Database.DocumentDataEntities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

internal sealed class OfferData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public SimulationInputsData SimulationInputs { get; set; }
    public SimulationOutputsData SimulationOutputs { get; set; }
    public BasicParametersData BasicParameters { get; set; }

    public sealed class SimulationInputsData
    {
        public DateTime? ExpectedDateOfDrawing { get; set; }
        public int ProductTypeId { get; set; }
        public int LoanKindId { get; set; }
        public decimal? LoanAmount { get; set; }
        public int? LoanDuration { get; set; }
        public DateTime GuaranteeDateFrom { get; set; }
        public decimal? InterestRateDiscount { get; set; }
        public int? FixedRatePeriod { get; set; }
        public decimal CollateralAmount { get; set; }
        public int? DrawingTypeId  { get; set; }
        public int? DrawingDurationId  { get; set; }
        public int? PaymentDay  { get; set; }
        public bool? IsEmployeeBonusRequested  { get; set; }
        public DeveloperData? Developer  { get; set; }
        public List<LoanPurposeData>? LoanPurposes  { get; set; }
	    public FeeSettingsData? FeeSettings  { get; set; }
        public MarketingActionData? MarketingActions  { get; set; }
        public List<InputFeeData>? Fees { get; set; }
	    public FrequencyData? RiskLifeInsurance  { get; set; }
        public FrequencyData? RealEstateInsurance  { get; set; }
    }

    public sealed class SimulationOutputsData
    {
        public decimal? LoanAmount { get; set; }
        public int LoanDuration { get; set; }
        public DateTime? LoanDueDate { get; set; }
        public decimal? LoanPaymentAmount { get; set; }
        public decimal? LoanInterestRateProvided { get; set; }
        public int? EmployeeBonusLoanCode { get; set; }
        public decimal? LoanToValue { get; set; }
        public DateTime? ContractSignedDate { get; set; }
        public DateTime? DrawingDateTo { get; set; }
        public DateTime? AnnuityPaymentsDateFrom { get; set; }
        public int? AnnuityPaymentsCount { get; set; }
        public decimal? Aprc { get; set; }
        public decimal? LoanTotalAmount { get; set; }
        public decimal? LoanInterestRate { get; set; }
        public decimal? LoanInterestRateAnnounced { get; set; }
        public int LoanInterestRateAnnouncedType { get; set; }
        public decimal? EmployeeBonusDeviation { get; set; }
        public decimal? MarketingActionsDeviation { get; set; }
        public List<SimulationResultWarningData>? Warnings { get; set; }
    }

    public sealed class SimulationResultWarningData
    {
        public int? WarningCode { get; set; }
        public string? WarningText { get; set; }
        public string? WarningInternalMessage { get; set; }
    }

    public sealed class LoanPurposeData
    {
	    public int LoanPurposeId { get; set; }
	    public decimal? Sum { get;  set; }
    }

    public sealed class FeeSettingsData
    {
	    public int? FeeTariffPurpose { get; set; }
	    public bool IsStatementCharged { get; set; }
    }

    public sealed class MarketingActionData
    {
	    public bool Domicile { get; set; }
	    public bool HealthRiskInsurance { get; set; }
	    public bool RealEstateInsurance { get; set; }
	    public bool IncomeLoanRatioDiscount { get; set; }
	    public bool UserVip { get; set; }
    }

    public sealed class InputFeeData
    {
        public int FeeId { get; set; }
        public decimal? DiscountPercentage { get; set; }
    }

    public sealed class DeveloperData
    {
        public int? DeveloperId { get; set; }
        public int? ProjectId { get; set; }
        public string? Description { get; set; }
    }

    public sealed class FrequencyData
    {
        public decimal? Sum { get; set; }
        public int? Frequency { get; set; }
    }

    public sealed class BasicParametersData
    {
        public decimal? FinancialResourcesOwn { get; set; }
        public decimal? FinancialResourcesOther { get; set; }
        public DateTime? GuaranteeDateTo { get; set; }
        public int? StatementTypeId { get; set; }
    }
}
