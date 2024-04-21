namespace NOBY.Dto.Refinancing;

public abstract class RefinancingSimulationResult
{
    /// <summary>
    /// ID vytvorene simulace.
    /// </summary>
    public int OfferId { get; set; }

    /// <summary>
    /// Sazba
    /// </summary>
    /// <example>4.7</example>
    public decimal InterestRate { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>0.2</example>
    public decimal? InterestRateDiscount { get; set; }

    /// <summary>
    /// Sazba po slevě
    /// </summary>
    /// <example>4.5</example>
    public decimal? InterestRateDiscounted { get => InterestRateDiscount.HasValue ? InterestRate - InterestRateDiscount : null; }

    /// <summary>
    /// Výše měsíční splátky.
    /// </summary>
    public decimal LoanPaymentAmount { get; set; }

    /// <summary>
    /// Výše měsíční splátky se zohledněním IC.
    /// </summary>
    public decimal? LoanPaymentAmountDiscounted { get; set; }
}
