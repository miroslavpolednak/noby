using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixationOfferList;

public sealed class SimulateMortgageRefixationOfferListRequest
    : IRequest<SimulateMortgageRefixationOfferListResponse>
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>0.09</example>
    public decimal? InterestRateDiscount { get; set; }

    internal SimulateMortgageRefixationOfferListRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
