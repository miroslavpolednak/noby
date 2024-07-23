namespace NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters.Dto;

public sealed class RefinancingParamCurrent
{
    /// <summary>
    /// Výše úvěru
    /// </summary>
    public decimal? LoanAmount { get; set; }

    /// <summary>
    /// Aktuální zůstatek jistiny
    /// </summary>
    public decimal? Principal { get; set; }

    /// <summary>
    /// Datum splatnosti - předpoklad/skutečnost
    /// </summary>
    public DateTime? LoanDueDate { get; set; }

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
    /// Platnost úrokové sazby od (spočítaná jako fixedRateValidTo mínus fixedRatePeriod)
    /// </summary>
    public DateTime? FixedRateValidFrom { get; set; }

    /// <summary>
    /// Platnost úrokové sazby do
    /// </summary>
    public DateTime? FixedRateValidTo { get; set; }
}
