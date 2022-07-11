namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class Amount
{
    [ProtoMember(1)]
    public decimal? Value { get; set; }

    [ProtoMember(2)]
    public string? CurrencyCode { get; set; }
}