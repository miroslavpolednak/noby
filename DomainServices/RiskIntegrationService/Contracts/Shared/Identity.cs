namespace DomainServices.RiskIntegrationService.Contracts;

[DataContract]
public class Identity
{
    [DataMember(Order = 1)]
    public string? IdentityId { get; set; }

    [DataMember(Order = 2)]
    public string? IdentityScheme { get; set; }
}
