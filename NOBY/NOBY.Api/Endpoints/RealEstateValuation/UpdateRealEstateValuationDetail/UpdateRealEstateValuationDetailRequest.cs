using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NOBY.Dto.Attributes;
using NOBY.Dto.RealEstateValuation;
using NOBY.Dto.RealEstateValuation.SpecificDetails;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateRealEstateValuationDetail;

public class UpdateRealEstateValuationDetailRequest : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    /// <summary>
    /// True pokud jde o nemovitost, která je objektem úvěru. Default: false
    /// </summary>
    /// <example>true</example>
    [Required, DefaultValue(false)]
    public bool IsLoanRealEstate { get; set; }

    /// <summary>
    /// ID stavu nemovitosti.
    /// </summary>
    /// <example>2</example>
    public RealEstateStateIds? RealEstateStateId { get; set; }

    /// <summary>
    /// Adresa nemovitosti
    /// </summary>
    /// <example>Novákovo nábřeží 1972, 521 71 Žďár nad Sázavou</example>
    public string? Address { get; set; }

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
    [SwaggerOneOf<HouseAndFlatDetails, ParcelDetails>]
    public object? SpecificDetails { get; set; }

    internal UpdateRealEstateValuationDetailRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}