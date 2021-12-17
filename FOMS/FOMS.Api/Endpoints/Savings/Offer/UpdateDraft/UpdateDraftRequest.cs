namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class UpdateDraftRequest
    : IRequest<SaveDraftResponse>
{
    public int OfferInstanceId { get; set; }

    public int SalesArrangementId { get; set; }
}
