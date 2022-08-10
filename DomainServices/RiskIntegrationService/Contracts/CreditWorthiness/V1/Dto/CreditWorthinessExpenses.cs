namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V1;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class CreditWorthinessExpenses
{
    [ProtoMember(1)]
    public decimal? Rent { get; set; }

    [ProtoMember(2)]
    public decimal? Saving { get; set; }

    [ProtoMember(3)]
    public decimal? Insurance { get; set; }

    [ProtoMember(4)]
    public decimal? Other { get; set; }
}
