namespace NOBY.Api.Endpoints.Refinancing.GetAvailableFixedRatePeriods;

public sealed class GetAvailableFixedRatePeriodsResponse
{
    /// <summary>
    /// Dostupné délky fixace pro daný Case v měsících
    /// </summary>
    public List<int>? AvailableFixedRatePeriods { get; set; }
}
