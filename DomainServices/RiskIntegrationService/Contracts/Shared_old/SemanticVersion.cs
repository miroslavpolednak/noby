namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class SemanticVersion
{
    [ProtoMember(1)]
    public int Major { get; set; }

    [ProtoMember(2)]
    public int Minor { get; set; }

    [ProtoMember(3)]
    public int Bugfix { get; set; }

    [ProtoMember(4)]
    public string? NonSemanticPart { get; set; }
}
