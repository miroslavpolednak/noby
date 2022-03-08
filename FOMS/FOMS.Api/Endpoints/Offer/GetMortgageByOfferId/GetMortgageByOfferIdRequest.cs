using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.GetMortgageByOfferId;

internal record GetMortgageByOfferIdRequest(int OfferId)
    : IRequest<Dto.GetMortgageResponse>
{ }
