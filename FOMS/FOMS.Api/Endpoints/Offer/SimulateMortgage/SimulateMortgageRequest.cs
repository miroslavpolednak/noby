using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.SimulateMortgage;

public sealed class SimulateMortgageRequest
    : Dto.MortgageInputs, IRequest<SimulateMortgageResponse>, IValidatableRequest
{
    /// <summary>
    /// Unikatni identifikator pro session simulace.
    /// Musi byt parsovatelny na .NET Guid type.
    /// </summary>
    public string? ResourceProcessId { get; set; }
}
