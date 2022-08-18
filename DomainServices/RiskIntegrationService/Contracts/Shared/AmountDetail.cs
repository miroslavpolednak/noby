namespace DomainServices.RiskIntegrationService.Contracts.Shared;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class AmountDetail
{
    [ProtoMember(1)]
    public decimal Amount { get; set; }

    [ProtoMember(2)]
    public string? CurrencyCode { get; set; }
}
