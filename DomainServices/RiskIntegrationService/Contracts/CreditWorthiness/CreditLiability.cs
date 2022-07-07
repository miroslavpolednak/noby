namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class CreditLiability
{
    [ProtoMember(1)]
    public int LiabilityType { get; set; }

    [ProtoMember(2)]
    public decimal? Limit { get; set; }

    [ProtoMember(3)]
    public decimal? AmountConsolidated { get; set; }

    [ProtoMember(4)]
    public decimal? Installment { get; set; }

    [ProtoMember(5)]
    public decimal? InstallmentConsolidated { get; set; }

    [ProtoMember(6)]
    public bool OutHomeCompanyFlag { get; set; }
}
