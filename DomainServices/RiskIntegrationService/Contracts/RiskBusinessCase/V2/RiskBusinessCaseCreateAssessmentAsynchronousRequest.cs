using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
public class RiskBusinessCaseCreateAssessmentAsynchronousRequest
    : IRequest<_V2.RiskBusinessCaseCreateAssessmentAsynchronousResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public string RiskBusinessCaseId { get; set; } = null!;

    [ProtoMember(2)]
    public long SalesArrangementId { get; set; }

    [ProtoMember(3)]
    public string LoanApplicationDataVersion { get; set; } = default!;

    [ProtoMember(4)]
    public Shared.ItChannels ItChannelPrevious { get; set; }

    [ProtoMember(5)]
    public RiskBusinessCaseAssessmentModes AssessmentMode { get; set; }

    [ProtoMember(6)]
    public RiskBusinessCaseGrantingProcedureCodes GrantingProcedureCode { get; set; }

    [ProtoMember(7)]
    public bool SelfApprovalRequired { get; set; }

    [ProtoMember(8)]
    public bool SystemApprovalRequired { get; set; }

    [ProtoMember(9)]
    public List<Shared.V1.LoanApplicationException>? LoanApplicationExceptions { get; set; }

    [ProtoMember(10)]
    public string? ExceptionHighestApprovalLevel { get; set; }
}
