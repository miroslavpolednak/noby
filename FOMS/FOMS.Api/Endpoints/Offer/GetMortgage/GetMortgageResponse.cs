namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class GetMortgageResponse
    : OfferInstance
{
    public MortgageInputs Inputs { get; set; }
    public MortgageResults Resulsts { get; set; }
}
