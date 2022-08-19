namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentReason
{
    [ProtoMember(1)]
    public string? Code { get; set; }

    [ProtoMember(2)]
    public string? Level { get; set; }

    [ProtoMember(3)]
    public long? Weight { get; set; }

    [ProtoMember(4)]
    public string? Category { get; set; }

    [ProtoMember(5)]
    public string? Target { get; set; }

    [ProtoMember(6)]
    public string? Description { get; set; }

    [ProtoMember(7)]
    public string? Result { get; set; }
}
