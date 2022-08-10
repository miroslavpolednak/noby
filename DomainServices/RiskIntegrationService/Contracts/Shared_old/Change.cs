namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class Change
{
    [ProtoMember(1)]
    public string? IdentityId { get; set; }

    [ProtoMember(2)]
    public DateTime? Date { get; set; }
}
