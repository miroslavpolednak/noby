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
    /// <example>true</example>
    [Required]
    public bool CaseInProgress { get; set; }

    /// <summary>
    /// ID Upřesnění typu nemovitosti. Slouží jako podklad pro překlad typu nemovitosti do ACV.
    /// </summary>
    /// <example>1</example>
    public int? RealEstateSubtypeId { get; set; }

    public LoanPurposeDetail? LoanPurposeDetails { get; set; }

    /// <summary>
    /// Objekty SpecificDetails jsou řízeny business logikou <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=644560135">Ocenění nemovitosti - varianty nemovitostí</a>.<br />
    /// Objekt HouseAndFlatDetails bude použit v případě, že jde o variantu nemovitosti HF.<br />
    /// Objekt ParcelDetails bude použit v případě, že jde o variantu nemovitosti P.<br />
    /// Pokud jde o variantu nemovitosti O, nebude použit ani jeden z objektů SpecificDetails.
    /// </summary>
    [SwaggerOneOf<SpecificDetails.HouseAndFlatDetails, SpecificDetails.ParcelDetails>]
    public object? SpecificDetails { get; set; }
}