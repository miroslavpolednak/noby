using CIS.Foms.Enums;
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
    /// Název typu Ocenění nemovitosti. 0 - Unknown, 1 - Online, 2 - DTS, 3 - Standard
    /// </summary>
    [Required]
    public RealEstateValuationTypes ValuationTypeId { get; set; }

    public OrderRealEstateValuationLocalSurveyPerson? LocalSurveyPerson { get; set; }

    internal OrderRealEstateValuationRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}

/// <summary>
/// Kontakt pro místní šetření
/// </summary>
public sealed class OrderRealEstateValuationLocalSurveyPerson
{
    /// <summary>
    /// Hodnota z číselníku RealEstateValuationLocalSurveyFunction
    /// </summary>
    /// <example>PRODAV</example>
    [Required]
    [MinLength(1)]
    public string FunctionCode { get; set; } = string.Empty;

    /// <summary>
    /// Jméno
    /// </summary>
    /// <example>Jidáš</example>
    [Required]
    [MinLength(1)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Příjmení
    /// </summary>
    /// <example>Skočdopole</example>
    [Required]
    [MinLength(1)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public NOBY.Dto.EmailAddressDto EmailAddress { get; set; }

    [Required]
    public NOBY.Dto.PhoneNumberDto MobilePhone { get; set; }
}
