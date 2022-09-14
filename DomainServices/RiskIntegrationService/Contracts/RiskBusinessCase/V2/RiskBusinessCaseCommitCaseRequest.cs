namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
public class RiskBusinessCaseCommitCaseRequest
    : IRequest<RiskBusinessCaseCommitCaseResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public string RiskBusinessCaseId { get; set; } = default!;

    [ProtoMember(2)]
    public long SalesArrangementId { get; set; }

    [ProtoMember(3)]
    public int ProductTypeId { get; set; }

    [ProtoMember(4)]
    public RiskBusinessCaseFinalResults FinalResult { get; set; }

    [ProtoMember(5)]
    public Shared.IdIdentifier? SoldProduct { get; set; }

    [ProtoMember(6)]
    public string? ApprovalLevel { get; set; }

    [ProtoMember(7)]
    public DateTime? ApprovalDate { get; set; }

    [ProtoMember(8)]
    public RiskBusinessCaseLoanAgreement? LoanAgreement { get; set; }

    [ProtoMember(9)]
    public Shared.Identity? UserIdentity { get; set; }

    [ProtoMember(10)]
    public Shared.Identity? Approver { get; set; }

    [ProtoMember(11)]
    public List<string>? CollateralAgreementsId { get; set; }
}
