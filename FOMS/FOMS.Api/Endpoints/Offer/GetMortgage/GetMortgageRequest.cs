using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.Dto;

internal record GetMortgageRequest(int OfferId)
    : IRequest<GetMortgageResponse>, IValidatableRequest
{ }
