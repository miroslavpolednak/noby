namespace NOBY.Api.Endpoints.Refinancing.GetInterestRate;

public sealed class GetInterestRateResponse
{
    /// <summary>
    /// Aktuální úroková sazba.
    /// </summary>
    public decimal? LoanInterestRateCurrent { get; set; }
}
