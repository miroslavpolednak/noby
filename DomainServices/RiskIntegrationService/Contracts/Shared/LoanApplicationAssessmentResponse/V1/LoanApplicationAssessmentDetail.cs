
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentDetail
{
    [ProtoMember(1)]
    public LoanApplicationAssessmentScore? Score { get; set; }

    [ProtoMember(2)]
    public LoanApplicationAssesmentLimit? Limit { get; set; }

    [ProtoMember(3)]
    public LoanApplicationAssesmentRiskCharacteristics? RiskCharacteristics { get; set; }
}
