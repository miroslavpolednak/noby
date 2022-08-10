namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class HouseholdAssessmentDetail
{
    [ProtoMember(1)]
    public long? HouseholdId { get; set; }

    [ProtoMember(2)]
    public AssessmentDetail? AssessmentDetail { get; set; }
}
