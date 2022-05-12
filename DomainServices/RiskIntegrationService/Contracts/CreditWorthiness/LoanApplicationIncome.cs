namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Příjem klienta
/// </summary>
[DataContract]
public class LoanApplicationIncome
{
    /// <summary>
    /// Kategorie příjmu.
    /// </summary>
    [DataMember(Order = 1)]
    public int CategoryMp { get; set; }

    /// <summary>
    /// Výše příjmu.
    /// </summary>
    [DataMember(Order = 2)]
    public double Amount { get; set; }
}
