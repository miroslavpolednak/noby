namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class ResourceIdentifier
{
    [ProtoMember(1)]
    public string? Instance { get; set; }

    [ProtoMember(2)]
    public string? Domain { get; set; }

    [ProtoMember(3)]
    public string? Resource { get; set; }

    [ProtoMember(4)]
    public string? Id { get; set; }

    [ProtoMember(5)]
    public string? Variant { get; set; }
}
