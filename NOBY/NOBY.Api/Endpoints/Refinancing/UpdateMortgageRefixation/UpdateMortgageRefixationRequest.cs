using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;

public sealed class UpdateMortgageRefixationRequest
    : IRequest
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    [JsonIgnore]
    internal int SalesArrangementId { get; set; }



    internal UpdateMortgageRefixationRequest InfuseId(long caseId, int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        this.CaseId = caseId;
        return this;
    }
}
