namespace DomainServices.RiskIntegrationService.Contracts;

[DataContract]
public class CompanyId
{
    [DataMember(Order = 1)]
    public string Company { get; set; }

    [DataMember(Order = 2)]
    public string Id { get; set; }
}
