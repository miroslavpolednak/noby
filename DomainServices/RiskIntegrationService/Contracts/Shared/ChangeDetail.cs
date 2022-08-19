namespace DomainServices.RiskIntegrationService.Contracts.Shared;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class ChangeDetail
{
    [ProtoMember(1)]
    public string? IdentityId { get; set; }

    [ProtoMember(2)]
    public DateTime? ChangeTime { get; set; }
}
