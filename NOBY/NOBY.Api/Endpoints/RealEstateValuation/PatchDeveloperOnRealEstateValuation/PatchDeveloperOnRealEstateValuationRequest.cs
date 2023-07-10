using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.PatchDeveloperOnRealEstateValuation;

/// <summary>
/// Editovatelný toggle developera na Ocenění nemovitosti
/// </summary>
public sealed class PatchDeveloperOnRealEstateValuationRequest
    : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    /// <summary>
    /// True pokud je aplikované hromadné ocenění z developerského projektu
    /// </summary>
    public bool DeveloperApplied { get; set; }

    internal PatchDeveloperOnRealEstateValuationRequest InfuseId(long caseId, int realEstateValuationId)
    {
        this.RealEstateValuationId = realEstateValuationId;
        this.CaseId = caseId;
        return this;
    }
}
