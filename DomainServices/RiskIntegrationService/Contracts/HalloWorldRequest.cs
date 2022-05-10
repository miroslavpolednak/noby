namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Request pro endpoint HalloWorld
/// </summary>
[DataContract]
public class HalloWorldRequest
    : IRequest<HalloWorldResponse>
{
    /// <summary>
    /// Ukazka komentare - ID requestu
    /// </summary>
    /// <remarks>Tohle idcko ted nepouzivam</remarks>
    /// <example>11</example>
    [DataMember(Order = 1)]
    public int Id { get; set; }

    /// <summary>
    /// Nazev osoby
    /// </summary>
    /// <example>John Doe</example>
    [DataMember(Order = 2)]
    public string? Name { get; set; }
}
