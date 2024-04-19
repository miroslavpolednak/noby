using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

public sealed class SimulateMortgageRetentionRequest
    : IRequest<SimulateMortgageRetentionResponse>
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    /// <summary>
    /// Platnost nové úrokové sazby od.
    /// </summary>
    [Required]
    public DateTime InterestRateValidFrom { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>0.09</example>
    public decimal? InterestRateDiscount { get; set; }

    /// <summary>
    /// Upravená výše poplatku. Relevantní pouze pro retence.
    /// </summary>
    public decimal? FeeAmountDiscounted { get; set; }

    internal SimulateMortgageRetentionRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
