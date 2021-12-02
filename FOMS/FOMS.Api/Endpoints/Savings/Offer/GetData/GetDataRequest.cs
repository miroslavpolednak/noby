namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal sealed record GetDataRequest(int OfferInstanceId)
    : IRequest<GetDataResponse>
{
}
