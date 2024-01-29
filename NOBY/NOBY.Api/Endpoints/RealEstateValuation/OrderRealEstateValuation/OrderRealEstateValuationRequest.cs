using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.OrderRealEstateValuation;

/// <summary>
/// Údaje nutné k objednání ocenění
/// </summary>
public sealed class OrderRealEstateValuationRequest
    : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    /// <summary>
    /// Název typu Ocenění nemovitosti. 1 - Online, 2 - DTS, 3 - Standard
    /// </summary>
    [Required]
    public RealEstateValuationTypes ValuationTypeId { get; set; }

    public Dto.RealEstateValuation.LocalSurveyData? LocalSurveyPerson { get; set; }

    internal OrderRealEstateValuationRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}
