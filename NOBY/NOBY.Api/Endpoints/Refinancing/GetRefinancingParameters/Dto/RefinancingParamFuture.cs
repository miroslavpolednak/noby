namespace NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters.Dto;

public sealed class RefinancingParamFuture
{
    /// <summary>
    /// Aktuální úroková sazba
    /// </summary>
    public decimal? LoanInterestRate { get; set; }

    /// <summary>
    /// Délka fixace úrokové sazby
    /// </summary>
    public int? FixedRatePeriod { get; set; }

    /// <summary>
    /// Výše měsíční splátky
    /// </summary>
    public decimal? LoanPaymentAmount { get; set; }

    /// <summary>
    /// Platnost úrokové sazby od
    /// </summary>
    public DateTime? FixedRateValidFrom { get; set; }

    /// <summary>
    /// Platnost úrokové sazby do
    /// </summary>
    public DateTime? FixedRateValidTo { get; set; }
}
