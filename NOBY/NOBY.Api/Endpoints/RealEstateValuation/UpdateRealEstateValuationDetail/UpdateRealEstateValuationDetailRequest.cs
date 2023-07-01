using System.Text.Json.Serialization;
using NOBY.Dto.RealEstateValuation.RealEstateValuationDetailDto;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateRealEstateValuationDetail;

public class UpdateRealEstateValuationDetailRequest : IRequest
{
    [JsonIgnore]
    public long CaseId { get; set; }

    [JsonIgnore]
    public int RealEstateValuationId { get; set; }

    /// <summary>
    /// True pokud jde o nemovitost, která je objektem úvěru. Default: false
    /// </summary>
    public bool IsLoanRealEstate { get; set; }

    /// <summary>
    /// ID stavu nemovitosti. 0 - Unknown, 1 - Dokončená, 2 - V rekonstrukci, 3 - Projekt, 4 - Výstavba
    /// </summary>
    public int? RealEstateStateId { get; set; }

    /// <summary>
    /// Adresa nemovitosti
    /// </summary>
    /// <example>ovákovo nábřeží 1972, 521 71 Žďár nad Sázavou</example>
    public string? Address { get; set; }

    /// <summary>
    /// ID Upřesnění typu nemovitosti. Slouží jako podklad pro překlad typu nemovitosti do ACV.
    /// </summary>
    public int? RealEstateSubtypeId { get; set; }

    /// <summary>
    /// Objekt je použit pouze pokud jde o Ocenění na case, který není v procesu (tedy pokud jde o HUBN nebo změnu).
    /// </summary>
    public LoanPurposeDetail? LoanPurposeDetails { get; set; }

    /// <summary>
    /// Objekty <see cref="SpecificDetails"/> jsou řízeny business logikou <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=644560135">Ocenění nemovitosti - varianty nemovitostí</a>.<br />
    /// Objekt <see cref="HouseAndFlatDetails"/> bude použit v případě, že jde o variantu nemovitosti HF. <br />
    /// Objekt <see cref="ParcelDetails"/> bude použit v případě, že jde o variantu nemovitosti P.<br />
    /// Pokud jde o variantu nemovitosti O, nebude použit ani jeden z objektů <see cref="SpecificDetails"/>.
    /// </summary>
    [JsonConverter(typeof(SpecificDetailsConverter))]
    public ISpecificDetails? SpecificDetails { get; set; }
}