namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationProduct
{
    [ProtoMember(1)]
    public int ProductTypeId { get; set; }

    [ProtoMember(2)]
    public int LoanKindId { get; set; }

    [ProtoMember(3)]
    public decimal? Ltv { get; set; }

    [ProtoMember(4)]
    public string? AplType { get; set; }

    [ProtoMember(5)]
    public List<LoanApplicationProductPurpose>? Purposes { get; set; }

    [ProtoMember(6)]
    public List<LoanApplicationProductCollateral>? Collaterals { get; set; }

    [ProtoMember(7)]
    public decimal? RequiredAmount { get; set; }

    [ProtoMember(8)]
    public decimal? InvestmentAmount { get; set; }

    [ProtoMember(9)]
    public decimal? OwnResourcesAmount { get; set; }

    [ProtoMember(10)]
    public decimal? ForeignResourcesAmount { get; set; }

    [ProtoMember(11)]
    public int? LoanDuration { get; set; }

    [ProtoMember(12)]
    public decimal? LoanPaymentAmount { get; set; }

    [ProtoMember(13)]
    public int? FixedRatePeriod { get; set; }

    [ProtoMember(14)]
    public decimal? LoanInterestRate { get; set; }

    [ProtoMember(15)]
    public int? RepaymentScheduleTypeId { get; set; }

    [ProtoMember(16)]
    public int? InstallmentCount { get; set; }

    [ProtoMember(17)]
    public DateTime? DrawingPeriodStart { get; set; }

    [ProtoMember(18)]
    public DateTime? DrawingPeriodEnd { get; set; }

    [ProtoMember(19)]
    public DateTime? RepaymentPeriodStart { get; set; }

    [ProtoMember(20)]
    public DateTime? RepaymentPeriodEnd { get; set; }

    [ProtoMember(21)]
    public List<int>? MarketingActions { get; set; }

    [ProtoMember(22)]
    public string? InstallmentPeriod { get; set; }

    [ProtoMember(23)]
    public string? HomeCurrencyIncome { get; set; }

    [ProtoMember(24)]
    public string? HomeCurrencyResidence { get; set; }

    [ProtoMember(25)]
    public int? DeveloperId { get; set; }

    [ProtoMember(26)]
    public int? DeveloperProjectId { get; set; }

    public List<string>? FinancingTypes { get; set; }
}
