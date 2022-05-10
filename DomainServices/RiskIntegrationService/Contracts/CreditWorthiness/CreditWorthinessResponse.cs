namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Vypočtená Bonita (výsledek)
/// </summary>
[DataContract]
public class CreditWorthinessResponse
{
    /// <summary>
    /// Maximální disponibilní splátka
    /// </summary>
    [DataMember(Order = 1)]
    public long? InstallmentLimit { get; set; }

    /// <summary>
    /// Maximální výše úvěru
    /// </summary>
    [DataMember(Order = 2)]
    public long? MaxAmount { get; set; }

    /// <summary>
    /// Zbývá na živobytí s ANNUITY
    /// </summary>
    [DataMember(Order = 3)]
    public long? RemainsLivingAnnuity { get; set; }

    /// <summary>
    /// Zbývá na živobytí s disp. Splátkou
    /// </summary>
    [DataMember(Order = 4)]
    public long? RemainsLivingInst { get; set; }

    /// <summary>
    /// Výsledek porovnání disponibilní a požadované splátky (0 - nebonitní, 1 - bonitní)
    /// </summary>
    [DataMember(Order = 5)]
    public int WorthinessResult { get; set; }

    /// <summary>
    /// ResultReason
    /// </summary>
    [DataMember(Order = 6)]
    public ResultReason ResultReason { get; set; }
}
