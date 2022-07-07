namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class LoanApplicationIncome
{
    [ProtoMember(1)]
    public int CategoryMp { get; set; }

    [ProtoMember(2)]
    public decimal Amount { get; set; }
}
