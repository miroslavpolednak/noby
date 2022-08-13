namespace DomainServices.RiskIntegrationService.Contracts.Shared;

[ProtoContract]
public class IdIdentifier
{
    [ProtoMember(1)]
    public string Id { get; set; } = null!;

    [ProtoMember(2)]
    public string? Company { get; set; }
}
