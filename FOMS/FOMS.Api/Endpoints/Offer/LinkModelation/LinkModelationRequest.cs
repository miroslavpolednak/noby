namespace FOMS.Api.Endpoints.Offer.LinkModelation;

public class LinkModelationRequest
    : IRequest
{
    public int SalesArrangementId { get; set; }

    public int OfferId { get; set; }

    public string? PhoneNumberForOffer { get; set; }

    public string? EmailForOffer { get; set; }
}
