using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.SetValuationTypeId;

/// <summary>
/// Hodnota valuationTypeId
/// </summary>
public sealed class SetValuationTypeIdRequest
    : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    /// <summary>
    /// Název typu Ocenění nemovitosti. 0 - Unknown, 1 - Online, 2 - DTS, 3 - Standard
    /// </summary>
    public RealEstateValuationValuationTypes ValuationTypeId { get; set; }

    internal SetValuationTypeIdRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}