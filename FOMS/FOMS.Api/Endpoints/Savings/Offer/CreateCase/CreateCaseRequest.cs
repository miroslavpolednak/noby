namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class CreateCaseRequest
    : IRequest<int>
{
    public int OfferInstanceId { get; init; }
    public CreateCase Request { get; init; }

    public CreateCaseRequest(int offerInstanceId, CreateCase request)
    {
        Request = request;
        OfferInstanceId = offerInstanceId;
    }
}
