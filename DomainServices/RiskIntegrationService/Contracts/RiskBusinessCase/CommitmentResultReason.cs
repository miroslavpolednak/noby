namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[DataContract]
public class CommitmentResultReason
{
    /// <summary>
    /// kód důvodu nespočtení výsledku.
    /// </summary>
    [DataMember(Order = 1)]
    public string? Code { get; set; }

    /// <summary>
    /// popis důvodu nespočtení výsledku.
    /// </summary>
    [DataMember(Order = 2)]
    public string? Desc { get; set; }
}
