
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentResponse
{
    [ProtoMember(1)]
    public string? LoanApplicationAssessmentId { get; set; }

    [ProtoMember(2)]
    public long? SalesArrangementId { get; set; }

    [ProtoMember(3)]
    public string? RiskBusinesscaseId { get; set; }

    [ProtoMember(4)]
    public DateTime? RiskBusinessCaseExpirationDate { get; set; }

    [ProtoMember(5)]
    public long? AssessmentResult { get; set; }

    [ProtoMember(6)]
    public decimal? StandardRiskCosts { get; set; }

    [ProtoMember(7)]
    public long? GlTableCode { get; set; }

    [ProtoMember(8)]
    public List<LoanApplicationAssessmentReason>? Reasons { get; set; }

    [ProtoMember(9)]
    public LoanApplicationAssessmentDetail? Detail { get; set; }

    [ProtoMember(10)]
    public List<LoanApplicationAssessmentHouseholdDetail>? HouseholdsDetails { get; set; }

    [ProtoMember(11)]
    public List<LoanApplicationAssessmentCustomerDetail>? CustomersDetails { get; set; }

    [ProtoMember(12)]
    public LoanApplicationAssessmentCollateralRiskCharacteristics? CollateralRiskCharacteristics { get; set; }

    [ProtoMember(13)]
    public LoanApplicationAssessmentApprovalPossibility? ApprovalPossibility { get; set; }

    [ProtoMember(14)]
    public string? Version { get; set; }

    [ProtoMember(15)]
    public ChangeDetail? Created { get; set; }

    [ProtoMember(16)]
    public ChangeDetail? Updated { get; set; }
}
