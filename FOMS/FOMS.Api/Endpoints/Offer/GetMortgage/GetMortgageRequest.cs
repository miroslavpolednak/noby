namespace FOMS.Api.Endpoints.Offer.Dto;

internal record GetMortgageRequest(int OfferInstanceId)
    : IRequest<GetMortgageResponse>
{ }
