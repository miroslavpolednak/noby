using NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters.Dto;

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