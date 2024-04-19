namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

public sealed class SimulateMortgageRetentionResponse
    : Dto.Refinancing.RefinancingSimulationResult
{
    /// <summary>
    /// Výše poplatku
    /// </summary>
    public decimal FeeAmount { get; set; }

    /// <summary>
    /// Upravená výše poplatku
    /// </summary>
    public decimal? FeeAmountDiscounted { get; set; }
}
