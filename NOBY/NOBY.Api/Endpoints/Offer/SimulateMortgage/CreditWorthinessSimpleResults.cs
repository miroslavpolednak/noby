namespace NOBY.Api.Endpoints.Offer.SimulateMortgage;


public class CreditWorthinessSimpleResults
{
    /// <summary>
    /// Výsledek bonity
    /// </summary>
    public WorthinessResult WorthinessResult { get; set; }

    /// <summary>
    /// Měsíční splátka (maximum)
    /// </summary>
    public int InstallmentLimit { get; set; }

    /// <summary>
    /// Měsíční splátka (maximum)
    /// </summary>
    public int MaxAmount { get; set; }

    /// <summary>
    /// Zbývá na živobytí s požadovanou splátkou
    /// </summary>
    public int RemainsLivingAnnuity { get; set; }

    /// <summary>
    /// Zbývá na živobytí s maximální splátkou
    /// </summary>
    public int RemainsLivingInst { get; set; }
}

public enum WorthinessResult
{
    Unknown,
    Success,
    Failed
}