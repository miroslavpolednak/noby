namespace DomainServices.RiskIntegrationService.Contracts.Shared;

[ProtoContract]
public sealed class Identity
{
    [ProtoMember(1)]
    public string? IdentityId { get; set; }

    [ProtoMember(2)]
    public string? IdentityScheme { get; set; }
}
