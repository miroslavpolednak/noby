using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixation;

public sealed class SimulateMortgageRefixationRequest
    : IRequest<List<Dto.Refinancing.RefinancingSimulationResult>>
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    /// <summary>
    /// Platnost nové úrokové sazby od.
    /// </summary>
    [Required]
    public List<int> FixedRatePeriods { get; set; } = [];

    public long? ProcessId { get; set; }

    internal SimulateMortgageRefixationRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
