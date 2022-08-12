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
    public string AplType { get; set; }

    [ProtoMember(5)]
    public List<LoanApplicationProductPurpose>? Purposes { get; set; }

    [ProtoMember(6)]
    public List<LoanApplicationProductCollateral>? Collaterals { get; set; }

    [ProtoMember(7)]
    public decimal? AmountRequired { get; set; }

    [ProtoMember(8)]
    public decimal? AmountInvestment { get; set; }

    [ProtoMember(9)]
    public decimal? AmountOwnResources { get; set; }

    [ProtoMember(10)]
    public decimal? AmountForeignResources { get; set; }

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
}
