namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class RiskBusinessCaseCommitCaseRequest
    : IRequest<RiskBusinessCaseCommitCaseResponse>, CIS.Core.Validation.IValidatableRequest
{
    [DataMember(Order = 1)]
    public string RiskBusinessCaseId { get; set; } = default!;

    [DataMember(Order = 2)]
    public long SalesArrangementId { get; set; }

    [DataMember(Order = 3)]
    public int ProductTypeId { get; set; }

    [DataMember(Order = 4)]
    public Shared.IdIdentifier? SoldProduct { get; set; }

    [DataMember(Order = 5)]
    public DateTime? ApprovalDate { get; set; }

    [DataMember(Order = 6)]
    public Shared.Identity? UserIdentity { get; set; }

    [DataMember(Order = 7)]
    public Shared.Identity? Approver { get; set; }

    [DataMember(Order = 8)]
    public List<int>? CollateralAgreementsId { get; set; }
}
