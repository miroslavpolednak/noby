namespace DomainServices.RiskIntegrationService.Contracts;

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
    public AssessmentReasonDetail? Detail { get; set; }
}
