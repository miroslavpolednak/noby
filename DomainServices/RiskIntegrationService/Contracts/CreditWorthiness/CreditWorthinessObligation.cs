namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class CreditWorthinessObligation
{
    [ProtoMember(1)]
    public int ObligationTypeId { get; set; }

    [ProtoMember(2)]
    public decimal? Amount { get; set; }

    [ProtoMember(3)]
    public decimal? AmountConsolidated { get; set; }

    [ProtoMember(4)]
    public bool IsObligationCreditorExternal { get; set; }
}
