namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public class SystemId
{
    [ProtoMember(1)]
    public string? Id { get; set; }

    [ProtoMember(2)]
    public string? Name { get; set; }
}
