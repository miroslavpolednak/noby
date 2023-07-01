using NOBY.Dto.RealEstateValuation.RealEstateValuationDetailDto;

namespace NOBY.Dto.RealEstateValuation;

public class RealEstateValuationDetail
{
    /// <summary>
    /// Varianta nemovitosti pro determinování relevantních atributů
    /// </summary>
    /// <example>HF</example>
    public string RealEstateVariant { get; init; } = null!;

    /// <summary>
    /// True pokud je daný case ve stavu InProgress
    /// </summary>
    public bool CaseInProgress { get; init; }

    /// <summary>
    /// ID Upřesnění typu nemovitosti. Slouží jako podklad pro překlad typu nemovitosti do ACV.
    /// </summary>
    /// <example>1</example>
    public int? RealEstateSubtypeId { get; init; }

    /// <summary>
    /// ID účelu úvěru podle číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=413109663">LoanPurpose (CIS_UCEL_UVERU_INT1)</a>
    /// </summary>
    public LoanPurposeDetail LoanPurposeDetails { get; init; } = new();

    /// <summary>
    /// Objekty SpecificDetails jsou řízeny business logikou <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=644560135">Ocenění nemovitosti - varianty nemovitostí</a>.<br />
    /// Objekt HouseAndFlatDetails bude použit v případě, že jde o variantu nemovitosti HF.<br />
    /// Objekt ParcelDetails bude použit v případě, že jde o variantu nemovitosti P.<br />
    /// Pokud jde o variantu nemovitosti O, nebude použit ani jeden z objektů SpecificDetails.
    /// </summary>
    public ISpecificDetails? SpecificDetails { get; init; }
}