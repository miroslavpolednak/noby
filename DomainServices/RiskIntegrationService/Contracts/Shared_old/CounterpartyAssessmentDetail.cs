namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class CounterpartyAssessmentDetail
{
    [ProtoMember(1)]
    public long? CounterPartyId { get; set; }

    [ProtoMember(2)]
    public string? CustomerIdMp { get; set; }

    [ProtoMember(3)]
    public AssessmentDetail? AssessmentDetail { get; set; }
}
