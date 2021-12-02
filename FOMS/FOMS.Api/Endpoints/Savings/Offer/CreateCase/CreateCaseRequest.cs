namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class CreateCaseRequest
    : IRequest<int>
{
    public int PartyId { get; init; }
    public int OfferInstanceId { get; init; }

    public CreateCaseRequest(int offerInstanceId, Dto.CreateCase request)
    {
        OfferInstanceId = offerInstanceId;
        PartyId = request.PartyId.GetValueOrDefault();
    }
}
