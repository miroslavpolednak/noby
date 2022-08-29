
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentApprovalPossibility
{
    [ProtoMember(1)]
    public bool? SelfApprovalPossible { get; set; }

    [ProtoMember(2)]
    public bool? AutoApprovalPossible { get; set; }
}
