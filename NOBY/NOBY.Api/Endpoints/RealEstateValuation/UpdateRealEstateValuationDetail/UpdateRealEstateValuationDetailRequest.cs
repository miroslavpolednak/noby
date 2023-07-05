using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NOBY.Api.Endpoints.RealEstateValuation.Shared.SpecificDetails;
using NOBY.Dto.Attributes;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateRealEstateValuationDetail;

public class UpdateRealEstateValuationDetailRequest : IRequest
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    [JsonIgnore]
    internal int RealEstateValuationId { get; set; }

    /// <summary>
    /// True pokud jde o nemovitost, která je objektem úvěru. Default: false
    /// </summary>
    [Required]
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

    public Shared.LoanPurposeDetail? LoanPurposeDetails { get; set; }

    [SwaggerOneOf<HouseAndFlatDetails, ParcelDetails>]
    public object? SpecificDetails { get; set; }

    internal UpdateRealEstateValuationDetailRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}