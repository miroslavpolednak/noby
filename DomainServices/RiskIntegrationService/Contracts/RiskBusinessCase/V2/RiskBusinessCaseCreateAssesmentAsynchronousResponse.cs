namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
public class RiskBusinessCaseCreateAssesmentAsynchronousResponse
{
    [ProtoMember(1)]
    public long CommandId { get; set; }
}
