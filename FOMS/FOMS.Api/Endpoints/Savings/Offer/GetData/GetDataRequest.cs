namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed record GetDataRequest(int OfferInstanceId)
    : IRequest<GetDataResponse>
{
}
