
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentDetail
{
    [ProtoMember(1)]
    public LoanApplicationAssessmentScore? Score { get; set; }

    [ProtoMember(2)]
    public LoanApplicationAssessmentLimit? Limit { get; set; }

    [ProtoMember(3)]
    public LoanApplicationAssessmentRiskCharacteristics? RiskCharacteristics { get; set; }
}
