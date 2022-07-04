namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

/// <summary>
/// Protistrana žádosti o půjčku.
/// </summary>
[DataContract]
public class LoanApplicationCounterParty
{
    /// <summary>
    /// ID klienta
    /// </summary>
    [DataMember(Order = 1)]
    public string? IdMp { get; set; }

    /// <summary>
    /// Je klient druhem/družkou?
    /// </summary>
    [DataMember(Order = 2)]
    public bool IsPartnerMp { get; set; }

    /// <summary>
    /// Rodinný stav.
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public int MaritalStatusMp { get; set; }

    /// <summary>
    /// Příjem klienta.
    /// </summary>
    [DataMember(Order = 4)]
    public List<LoanApplicationIncome>? LoanApplicationIncome { get; set; }

    /// <summary>
    /// Závazky klienta.
    /// </summary>
    [DataMember(Order = 5)]
    public List<CreditLiability>? CreditLiabilities { get; set; }
}
