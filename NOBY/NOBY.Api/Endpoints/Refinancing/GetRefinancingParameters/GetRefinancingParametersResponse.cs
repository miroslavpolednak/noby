using NOBY.Dto.Refinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters;

public sealed class GetRefinancingParametersResponse
{
    /// <summary>
    /// True, pokud existuje rozpracovaná relevantní žádost
    /// </summary>
    public bool IsAnotherSalesArrangementInProgress { get; set; }

    /// <summary>
    /// Cenová citlivost klienta
    /// </summary>
    public decimal? CustomerPriceSensitivity { get; set; }

    /// <summary>
    /// Riziko odchodovosti klienta
    /// </summary>
    public decimal? CustomerChurnRisk { get; set; }

    public RefinancingParamCurrent RefinancingParametersCurrent { get; set; } = null!;

    public RefinancingParamFuture RefinancingParametersFuture { get; set; } = null!;

    public IReadOnlyCollection<RefinancingProcess> RefinancingProcesses { get; set; } = null!;

}

public sealed class RefinancingParamFuture
{
    /// <summary>
    /// Aktuální úroková sazba.
    /// </summary>
    public decimal? LoanInterestRate { get; set; }

    /// <summary>
    /// Délka fixace úrokové sazby.
    /// </summary>
    public int? FixedRatePeriod { get; set; }

    /// <summary>
    /// Výše měsíční splátky.
    /// </summary>
    public decimal? LoanPaymentAmount { get; set; }

    /// <summary>
    /// Platnost úrokové sazby od (spočítaná jako fixedRateValidTo mínus fixedRatePeriod).
    /// </summary>
    public DateTime? FixedRateValidFrom { get; set; }

    /// <summary>
    /// Platnost úrokové sazby do.
    /// </summary>
    public DateTime? FixedRateValidTo { get; set; }
}

public sealed class RefinancingParamCurrent
{
    /// <summary>
    /// Výše úvěru.
    /// </summary>
    public decimal? LoanAmount { get; set; }

    /// <summary>
    /// Aktuální zůstatek jistiny.
    /// </summary>
    public decimal? Principal { get; set; }

    /// <summary>
    /// Datum splatnosti - předpoklad/skutečnost.
    /// </summary>
    public DateTime? LoanDueDate { get; set; }

    /// <summary>
    /// Aktuální úroková sazba.
    /// </summary>
    public decimal? LoanInterestRate { get; set; }

    /// <summary>
    /// Délka fixace úrokové sazby.
    /// </summary>
    public int? FixedRatePeriod { get; set; }

    /// <summary>
    /// Výše měsíční splátky.
    /// </summary>
    public decimal? LoanPaymentAmount { get; set; }

    /// <summary>
    /// Platnost úrokové sazby od (spočítaná jako fixedRateValidTo mínus fixedRatePeriod).
    /// </summary>
    public DateTime? FixedRateValidFrom { get; set; }

    /// <summary>
    /// Platnost úrokové sazby do.
    /// </summary>
    public DateTime? FixedRateValidTo { get; set; }
}

public sealed class RefinancingProcess
{
    /// <summary>
    /// SalesArrangementId se vrací pro SalesArrangementy vytvořené v NOBY
    /// </summary>
    public int? SalesArrangementId { get; set; }

    public ProcessDetail ProcessDetail { get; set; } = null!;
}
