namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

/// <summary>
/// Household Credit Liabilities OutHomeCompany.
/// </summary>
[DataContract]
public class CreditLiability
{
    /// <summary>
    /// Typ závazku.
    /// </summary>
    [DataMember(Order = 1)]
    public int LiabilityType { get; set; }

    /// <summary>
    /// Výše limitu kreditní karty nebo povoleného debetu.
    /// </summary>
    [DataMember(Order = 2)]
    public double Limit { get; set; }

    /// <summary>
    /// Konsolidovaná výše limitu kreditní karty nebo povoleného debetu.
    /// </summary>
    [DataMember(Order = 3)]
    public decimal AmountConsolidated { get; set; }

    /// <summary>
    /// Výše splátky spotřebitelského nebo hypotečního úvěru.
    /// </summary>
    [DataMember(Order = 4)]
    public double Installment { get; set; }

    /// <summary>
    /// Konsolidovaná výše splátky spotřebitelského nebo hypotečního úvěru.
    /// </summary>
    [DataMember(Order = 5)]
    public double InstallmentConsolidated { get; set; }

    /// <summary>
    /// Příznak, že závazek je veden u jiného peněžního ústavu (JPÚ), než v kterém je sjednáván úvěr.
    /// </summary>
    [DataMember(Order = 6)]
    public bool OutHomeCompanyFlag { get; set; }
}
