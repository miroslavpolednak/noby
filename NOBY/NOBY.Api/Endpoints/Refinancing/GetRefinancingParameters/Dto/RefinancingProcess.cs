using NOBY.Dto.Refinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters.Dto;

public sealed class RefinancingProcess
{
    /// <summary>
    /// SalesArrangementId se vrací pro SalesArrangementy vytvořené v NOBY
    /// </summary>
    public int? SalesArrangementId { get; set; }

    public ProcessDetail ProcessDetail { get; set; } = null!;
}
