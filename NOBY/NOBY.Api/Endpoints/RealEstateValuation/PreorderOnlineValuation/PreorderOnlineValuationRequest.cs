using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.PreorderOnlineValuation;

/// <summary>
/// Upřesňující údaje k předobjednávce online ocenění
/// </summary>
public sealed class PreorderOnlineValuationRequest
    : Dto.RealEstateValuation.OnlinePreorderData, IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    internal PreorderOnlineValuationRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}
