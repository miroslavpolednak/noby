using System.ComponentModel.DataAnnotations;
using NOBY.Api.Endpoints.Offer.Dto;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgage;

public sealed class SimulateMortgageRequest
    : Dto.MortgageInputs, IRequest<SimulateMortgageResponse>
{
    /// <summary>
    /// Unikatni identifikator pro session simulace.
    /// Musi byt parsovatelny na .NET Guid type.
    /// </summary>
    [Required]
    public string? ResourceProcessId { get; set; }

    /// <summary>
    /// Simulace s garancí úrokové sazby, default hodnota false
    /// </summary>
    public bool? WithGuarantee { get; set; }

    /// <summary>
    /// ID Sales Arrangement-u
    /// </summary>
    public int? SalesArrangementId { get; set; }

    public CreditWorthinessSimpleInputs? CreditWorthinessSimpleInputs { get; set; }
}
