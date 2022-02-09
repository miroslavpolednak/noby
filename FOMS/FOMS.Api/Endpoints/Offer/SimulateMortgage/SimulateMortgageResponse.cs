namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class SimulateMortgageResponse
{
    public int OfferId { get; set; }
    public string? ResourceProcessId { get; set; }
    public MortgageOutputs? Outputs { get; set; }
}
