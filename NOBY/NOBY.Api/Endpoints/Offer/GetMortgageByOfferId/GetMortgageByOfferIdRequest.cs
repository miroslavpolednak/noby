using CIS.Core.Validation;

namespace NOBY.Api.Endpoints.Offer.GetMortgageByOfferId;

internal record GetMortgageByOfferIdRequest(int OfferId)
    : IRequest<Dto.GetMortgageResponse>
{ }
