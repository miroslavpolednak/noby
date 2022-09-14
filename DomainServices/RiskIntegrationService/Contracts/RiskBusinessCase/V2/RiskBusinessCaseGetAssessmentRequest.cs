
namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
public class RiskBusinessCaseGetAssessmentRequest
    : IRequest<Shared.V1.LoanApplicationAssessmentResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public string LoanApplicationAssessmentId { get; set; } = default!;

    [ProtoMember(2)]
    public List<RiskBusinessCaseRequestedDetails>? RequestedDetails { get; set; }

    [ProtoMember(3)]
    public Shared.AmountDetail? Amount { get; set; }
}
