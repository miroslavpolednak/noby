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
    public int ValuationTypeId { get; set; }

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
    public string FunctionCode { get; set; } = string.Empty;

    /// <summary>
    /// Jméno
    /// </summary>
    /// <example>Jidáš</example>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Příjmení
    /// </summary>
    /// <example>Skočdopole</example>
    public string LastName { get; set; } = string.Empty;

    public NOBY.Dto.EmailAddressDto EmailAddress { get; set; }

    public NOBY.Dto.PhoneNumberDto MobilePhone { get; set; }
}
