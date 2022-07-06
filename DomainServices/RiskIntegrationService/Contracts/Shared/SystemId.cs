namespace DomainServices.RiskIntegrationService.Contracts;

[DataContract]
public class SystemId
{
    [DataMember(Order = 1)]
    public string Id { get; set; } = default!;

    [DataMember(Order = 2)]
    public string Name { get; set; } = default!;
}
