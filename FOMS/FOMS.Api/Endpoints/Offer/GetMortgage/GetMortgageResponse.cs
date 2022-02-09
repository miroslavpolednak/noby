namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class GetMortgageResponse
{
    public int OfferId { get; set; }
    public string ResourceProcessId { get; set; }
    public MortgageInputs Inputs { get; set; }
    public MortgageOutputs Outputs { get; set; }
}
