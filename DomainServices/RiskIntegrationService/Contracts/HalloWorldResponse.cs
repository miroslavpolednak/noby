namespace DomainServices.RiskIntegrationService.Contracts;

[DataContract]
public class HalloWorldResponse
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
}
