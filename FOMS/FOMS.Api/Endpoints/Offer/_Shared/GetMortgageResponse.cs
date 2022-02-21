namespace FOMS.Api.Endpoints.Offer.Dto;

public sealed class GetMortgageResponse
{
    /// <summary>
    /// ID simulace.
    /// </summary>
    public int OfferId { get; set; }

    /// <summary>
    /// Unikatni identifikator pro session simulace.
    /// </summary>
    public string ResourceProcessId { get; set; } = null!;

    /// <summary>
    /// Zadani simulace.
    /// </summary>
    public Dto.MortgageInputs Inputs { get; set; } = null!;
    
    /// <summary>
    /// Vysledky simulace.
    /// </summary>
    public Dto.MortgageOutputs Outputs { get; set; } = null!;
}
