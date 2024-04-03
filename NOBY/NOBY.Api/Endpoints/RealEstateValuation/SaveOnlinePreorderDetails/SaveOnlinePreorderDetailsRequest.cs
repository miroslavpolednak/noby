using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveOnlinePreorderDetails;

/// <summary>
/// Detaily k online ocenění
/// </summary>
public sealed class SaveOnlinePreorderDetailsRequest
    : Dto.RealEstateValuation.OnlinePreorderData, IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    internal SaveOnlinePreorderDetailsRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}
