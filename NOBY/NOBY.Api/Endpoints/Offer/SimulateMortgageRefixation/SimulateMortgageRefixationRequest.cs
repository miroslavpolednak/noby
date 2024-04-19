using NOBY.Dto.Refinancing;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixation;

public sealed class SimulateMortgageRefixationRequest
    : IRequest<SimulateMortgageRefixationResponse>
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    /// <summary>
    /// Platnost nové úrokové sazby od.
    /// </summary>
    [Required]
    public int FixedRatePeriod { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>0.09</example>
    public decimal? InterestRateDiscount { get; set; }

    internal SimulateMortgageRefixationRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
