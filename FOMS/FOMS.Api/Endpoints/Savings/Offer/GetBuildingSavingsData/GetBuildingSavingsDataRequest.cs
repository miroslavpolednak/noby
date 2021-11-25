namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed record GetBuildingSavingsDataRequest(int OfferInstanceId)
    : IRequest<GetBuildingSavingsDataResponse>
{
}
