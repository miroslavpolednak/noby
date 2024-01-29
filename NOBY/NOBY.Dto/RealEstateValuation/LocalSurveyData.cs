namespace NOBY.Dto.RealEstateValuation;

/// <summary>
/// Kontakt pro místní šetření
/// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class LocalSurveyData
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
    public EmailAddressDto EmailAddress { get; set; }

    [Required]
    public PhoneNumberDto MobilePhone { get; set; }
}
