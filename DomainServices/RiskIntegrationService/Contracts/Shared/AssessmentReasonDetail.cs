namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class AssessmentReasonDetail
{
    [ProtoMember(1)]
    public string? Target { get; set; }

    [ProtoMember(2)]
    public string? Desc { get; set; }

    [ProtoMember(3)]
    public string? Result { get; set; }

    [ProtoMember(4)]
    public List<Resource>? Resource { get; set; }
}
