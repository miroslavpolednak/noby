namespace DomainServices.RiskIntegrationService.Contracts;

[DataContract]
public class HumanUser
{
    [DataMember(Order = 1)]
    public string? Identity { get; set; }

    [DataMember(Order = 2)]
    public string? IdentityScheme { get; set; }
}
