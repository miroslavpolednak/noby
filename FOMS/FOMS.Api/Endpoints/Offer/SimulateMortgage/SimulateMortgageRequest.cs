using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class SimulateMortgageRequest
    : MortgageInputs, IRequest<SimulateMortgageResponse>, IValidatableRequest
{
    public string? ResourceProcessId { get; set; }
}
