namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Parametry potřebné pro výpočet Bonity
/// </summary>
[DataContract]
public class LoanApplicationProduct
{
    /// <summary>
    /// Kód produktu
    /// </summary>
    [DataMember(Order = 1)]
    public int Product { get; set; }

    /// <summary>
    /// Doba splatnosti úvěru v měsících.
    /// </summary>
    [DataMember(Order = 2)]
    public int Maturity { get; set; }

    /// <summary>
    /// Žádaná roční úroková sazba.
    /// </summary>
    [DataMember(Order = 3)]
    public double InterestRate { get; set; }

    /// <summary>
    /// Požadovaná výše úvěru v Kč.
    /// </summary>
    [DataMember(Order = 4)]
    public int AmountRequired { get; set; }

    /// <summary>
    /// Požadovaná výše splátky v Kč.
    /// </summary>
    [DataMember(Order = 5)]
    public int Annuity { get; set; }

    /// <summary>
    /// Doba fixace úrokové sazby v měsících.
    /// </summary>
    [DataMember(Order = 6)]
    public int FixationPeriod { get; set; }
}
