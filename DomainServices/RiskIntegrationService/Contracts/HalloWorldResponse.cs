namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Response pro endpoint HalloWorld
/// </summary>
[DataContract]
public class HalloWorldResponse
{
    /// <summary>
    /// Nazev osoby
    /// </summary>
    [DataMember(Order = 1)]
    public string? Name { get; set; }
}
