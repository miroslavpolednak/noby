namespace DomainServices.RiskIntegrationService.Contracts;

[DataContract]
public class CompanyId
{
    /// <summary>
    /// Company (eg. MP, KB) 
    /// </summary>
    /// <value>Company (eg. MP, KB)</value>
    [DataMember(Order = 1)]
    public string Company { get; set; }

    /// <summary>
    /// Id
    /// </summary>
    /// <value>Id</value>
    [DataMember(Order = 2)]
    public string Id { get; set; }
}
