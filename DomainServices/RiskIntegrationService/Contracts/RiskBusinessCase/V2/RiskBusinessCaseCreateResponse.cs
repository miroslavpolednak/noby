namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
public class RiskBusinessCaseCreateResponse
{
    [ProtoMember(1)]
    public string RiskBusinessCaseId { get; set; } = null!;
}
