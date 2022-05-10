namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Důvod(y) nespočtení výsledků bonity.
/// </summary>
[DataContract]
public class ResultReason
{
    /// <summary>
    /// Kód důvodu nespočtení výsledku
    /// </summary>
    [DataMember(Order = 1)]
    public string? Code { get; set; }

    /// <summary>
    /// Popis důvodu nespočtení výsledku
    /// </summary>
    [DataMember(Order = 2)]
    public string? Description { get; set; }
}
