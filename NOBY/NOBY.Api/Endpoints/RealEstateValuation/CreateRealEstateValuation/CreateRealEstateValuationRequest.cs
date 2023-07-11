using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuation;

/// <summary>
/// Atributy k nově zakládanému Ocenění nemovitosti
/// </summary>
public sealed class CreateRealEstateValuationRequest
    : IRequest<int>
{
    [JsonIgnore]
    internal long CaseId;

    /// <summary>
    /// ID typu nemovitosti
    /// </summary>
    /// <example>1</example>
    [Required]
    public int RealEstateTypeId { get; set; }

    /// <summary>
    /// True pokud jde o nemovitost, která je objektem úvěru
    /// </summary>
    /// <example>true</example>
    [Required]
    public bool IsLoanRealEstate { get; set; }

    /// <summary>
    /// True pokud je aplikované hromadné ocenění z developerského projektu
    /// </summary>
    /// <example>false</example>
    [Required]
    public bool DeveloperApplied { get; set; }

    internal CreateRealEstateValuationRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
