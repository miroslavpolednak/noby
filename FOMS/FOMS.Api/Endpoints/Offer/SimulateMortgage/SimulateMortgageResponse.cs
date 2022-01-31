namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class SimulateMortgageResponse
    : OfferInstance
{
    public MortgageResults Results { get; set; }
}
