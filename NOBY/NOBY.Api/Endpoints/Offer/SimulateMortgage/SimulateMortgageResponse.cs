namespace NOBY.Api.Endpoints.Offer.SimulateMortgage;

public sealed class SimulateMortgageResponse
{
    /// <summary>
    /// ID vytvorene simulace.
    /// </summary>
    public int OfferId { get; set; }
    
    /// <summary>
    /// Unikatni identifikator pro session simulace.
    /// </summary>
    public string? ResourceProcessId { get; set; }
    
    /// <summary>
    /// Vysledky simulace.
    /// </summary>
    public Dto.MortgageOutputs? SimulationResults { get; set; }
}
