namespace NOBY.Api.Endpoints.Offer.CancelOffer;
internal sealed record CancelOfferRequest(long CaseId, int OfferId) : IRequest;