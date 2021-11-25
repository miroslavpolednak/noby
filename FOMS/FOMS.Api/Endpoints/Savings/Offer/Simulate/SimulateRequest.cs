using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class SimulateRequest 
    : BuildingSavingsInput, IRequest<OfferInstance>, IValidatableRequest
{
}
