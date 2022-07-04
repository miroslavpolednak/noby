namespace DomainServices.RiskIntegrationService.Contracts;

[DataContract]
public class SystemId
{
    /// <summary>
    /// Id
    /// </summary>
    /// <value>Id</value>
    [DataMember(Order = 1)]
    public string Id { get; set; } = default!;

    /// <summary>
    /// System (eg. NOBY, STARBUILD)
    /// </summary>
    /// <value>System (eg. NOBY, STARBUILD)</value>
    [DataMember(Order = 2)]
    public string Name { get; set; } = default!;
}
