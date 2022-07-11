namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class Resource
{
    [ProtoMember(1)]
    public string? Entity { get; set; }

    [ProtoMember(2)]
    public ResourceIdentifier? Id { get; set; }
}
