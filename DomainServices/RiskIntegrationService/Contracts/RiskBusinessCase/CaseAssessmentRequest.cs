namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[ProtoContract]
public class CaseAssessmentRequest
    : IRequest<CaseAssessmentResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public SystemId? LoanApplicationIdMp { get; set; }

    [ProtoMember(2)]
    public string? LoanApplicationDataVersion { get; set; }

    [ProtoMember(3)]
    public string? ItChannel { get; set; }

    [ProtoMember(4)]
    public string? ItChannelPrevious { get; set; }

    [ProtoMember(5)]
    public string? AssessmentMode { get; set; }

    [ProtoMember(6)]
    public string? GrantingProcedureCode { get; set; }

    [ProtoMember(7)]
    public bool SelfApprovalRequired { get; set; }

    [ProtoMember(8)]
    public bool SystemApprovalRequired { get; set; }

    [ProtoMember(9)]
    public List<LoanApplicationException>? LoanApplicationException { get; set; }

    [ProtoMember(10)]
    public string? ExceptionHighestApprovalLevel { get; set; }

    [ProtoMember(11)]
    public List<string>? Expand { get; set; }
}
