namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class CaseAssessmentResponse
{
    [ProtoMember(1)]
    public string? Id { get; set; }

    [ProtoMember(2)]
    public SystemId? LoanApplicationIdMp { get; set; }

    [ProtoMember(3)]
    public string? RiskBusinesscaseIdMp { get; set; }

    [ProtoMember(4)]
    public DateTime? RiskBusinesscaseExpirationDate { get; set; }

    [ProtoMember(5)]
    public long? AssessmentResult { get; set; }

    [ProtoMember(6)]
    public decimal? StandardRiskCosts { get; set; }

    [ProtoMember(7)]
    public long? GlTableCode { get; set; }

    [ProtoMember(8)]
    public List<LoanApplicationAssessmentReason>? LoanApplicationAssessmentReason { get; set; }

    [ProtoMember(9)]
    public AssessmentDetail? AssessmentDetail { get; set; }

    [ProtoMember(10)]
    public List<HouseholdAssessmentDetail>? HouseholdAssessmentDetail { get; set; }

    [ProtoMember(11)]
    public List<CounterpartyAssessmentDetail>? CounterpartyAssessmentDetail { get; set; }

    [ProtoMember(12)]
    public CollateralRiskCharacteristics? CollateralRiskCharacteristics { get; set; }

    [ProtoMember(13)]
    public SemanticVersion? Version { get; set; }

    [ProtoMember(14)]
    public Change? Created { get; set; }

    [ProtoMember(15)]
    public Change? Updated { get; set; }
}
