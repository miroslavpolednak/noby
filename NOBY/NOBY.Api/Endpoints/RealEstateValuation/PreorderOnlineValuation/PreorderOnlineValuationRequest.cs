using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.PreorderOnlineValuation;

/// <summary>
/// Upřesňující údaje k předobjednávce online ocenění
/// </summary>
public sealed class PreorderOnlineValuationRequest
    : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    /// <summary>
    /// Kód technického stavu budovy
    /// </summary>
    /// <example>1</example>
    [Required]
    public string BuildingTechnicalStateCode { get; set; } = string.Empty;

    /// <summary>
    /// Kód konstrukčního materiálu budovy
    /// </summary>
    /// <example>CIHLA</example>
    [Required]
    public string BuildingMaterialStructureCode { get; set; } = string.Empty;

    /// <summary>
    /// Kód dispozice
    /// </summary>
    /// <example>1+KK</example>
    [Required]
    public string FlatSchemaCode { get; set; } = string.Empty;

    /// <summary>
    /// Čistá podlahová plocha bytu
    /// </summary>
    [Required]
    public decimal FlatArea { get; set; }

    /// <summary>
    /// Kód stáří budovy
    /// </summary>
    /// <example>0</example>
    [Required]
    public string BuildingAgeCode { get; set; } = string.Empty;

    internal PreorderOnlineValuationRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}
