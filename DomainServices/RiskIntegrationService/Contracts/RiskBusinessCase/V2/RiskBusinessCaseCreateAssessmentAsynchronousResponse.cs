namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
public class RiskBusinessCaseCreateAssessmentAsynchronousResponse
{
    [ProtoMember(1)]
    public long CommandId { get; set; }
}
