namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[DataContract]
public class CreateCaseResponse
{
    [DataMember(Order = 1)]
    public string RiskBusinessCaseId { get; set; } = null!;
}
