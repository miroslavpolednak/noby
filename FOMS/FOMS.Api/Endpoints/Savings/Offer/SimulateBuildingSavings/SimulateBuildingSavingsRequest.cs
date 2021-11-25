using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class SimulateBuildingSavingsRequest 
    : BuildingSavingsInput, IRequest<OfferInstance>, IValidatableRequest
{
}
