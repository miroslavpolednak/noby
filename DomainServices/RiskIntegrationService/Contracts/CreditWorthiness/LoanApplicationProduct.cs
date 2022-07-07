namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class LoanApplicationProduct
{
    [ProtoMember(1)]
    public int Product { get; set; }

    [ProtoMember(2)]
    public int Maturity { get; set; }

    [ProtoMember(3)]
    public decimal InterestRate { get; set; }

    [ProtoMember(4)]
    public int AmountRequired { get; set; }

    [ProtoMember(5)]
    public int Annuity { get; set; }

    [ProtoMember(6)]
    public int FixationPeriod { get; set; }
}
