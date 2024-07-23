namespace NOBY.Api.Endpoints.Refinancing.GetInterestRatesValidFrom;

public sealed class GetInterestRatesValidFromResponse
{
    public List<GetInterestRatesValidFromResponseItem>? InterestRatesValidFrom { get; set; }
}

public sealed class GetInterestRatesValidFromResponseItem
{
    /// <summary>
    /// Platnost nové úrokové sazby od (nabízená hodnota).
    /// </summary>
    public DateTime InterestRateValidFrom { get; set; }

    /// <summary>
    /// Indikuje defaultní volbu pro výběr na FE.
    /// </summary>
    public bool IsDefault { get; set; }
}
