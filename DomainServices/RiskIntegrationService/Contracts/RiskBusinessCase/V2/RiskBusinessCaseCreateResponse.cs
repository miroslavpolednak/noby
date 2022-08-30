namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[DataContract]
public class RiskBusinessCaseCreateResponse
{
    [DataMember(Order = 1)]
    public string RiskBusinessCaseId { get; set; } = null!;
}
