namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Parametry domácnosti.
/// </summary>
[DataContract]
public class ExpensesSummary
{
    /// <summary>
    /// Náklady domácnosti spojené s bydlením (Kč/měsíc).
    /// </summary>
    [DataMember(Order = 1)]
    public double Rent { get; set; }

    /// <summary>
    /// Výdaje domácnosti na spoření (Kč/měsíc).
    /// </summary>
    [DataMember(Order = 2)]
    public double Saving { get; set; }

    /// <summary>
    /// Výdaje domácnosti na pojištění (Kč/měsíc).
    /// </summary>
    [DataMember(Order = 3)]
    public double Insurance { get; set; }

    /// <summary>
    /// Ostatní výdaje domácnosti (Kč/měsíc).
    /// </summary>
    [DataMember(Order = 4)]
    public double Other { get; set; }
}
