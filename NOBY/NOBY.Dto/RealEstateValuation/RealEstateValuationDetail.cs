using System.ComponentModel.DataAnnotations;
using NOBY.Dto.Attributes;

namespace NOBY.Dto.RealEstateValuation;

/// <summary>
/// Detail Ocenění nemovitosti
/// </summary>
public sealed class RealEstateValuationDetail
{
    /// <summary>
    /// Varianta nemovitosti pro determinování relevantních atributů
    /// </summary>
    /// <example>HF</example>
    [Required]
    public string RealEstateVariant { get; set; } = null!;

    /// <summary>
    /// True pokud je daný case ve stavu InProgress
    /// </summary>
    [Required]
    public bool CaseInProgress { get; set; }

    /// <summary>
    /// ID Upřesnění typu nemovitosti. Slouží jako podklad pro překlad typu nemovitosti do ACV.
    /// </summary>
    /// <example>1</example>
    public int? RealEstateSubtypeId { get; set; }

    public LoanPurposeDetail? LoanPurposeDetails { get; set; }

    [SwaggerOneOf<SpecificDetails.HouseAndFlatDetails, SpecificDetails.ParcelDetails>]
    public object? SpecificDetails { get; set; }
}