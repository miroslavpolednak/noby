namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class AssessmentDetail
{
    [ProtoMember(1)]
    public LoanApplicationScore? LoanApplicationScore { get; set; }

    [ProtoMember(2)]
    public LoanApplicationLimit? LoanApplicationLimit { get; set; }

    [ProtoMember(3)]
    public RiskCharacteristics? LoanApplicationRiskCharacteristics { get; set; }
}
