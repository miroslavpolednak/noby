using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.Offer.SimulateMortgage;

public sealed class SimulateMortgageRequest
    : Dto.MortgageInputs, IRequest<SimulateMortgageResponse>
{
    /// <summary>
    /// Unikatni identifikator pro session simulace.
    /// Musi byt parsovatelny na .NET Guid type.
    /// </summary>
    public string? ResourceProcessId { get; set; }

    /// <summary>
    /// Simulace s garancí úrokové sazby, default hodnota false
    /// </summary>
    public bool? WithGuarantee { get; set; }
}
