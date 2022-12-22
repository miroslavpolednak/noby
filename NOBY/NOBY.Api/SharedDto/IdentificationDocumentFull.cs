namespace NOBY.Api.SharedDto;

public class IdentificationDocumentFull
    : IdentificationDocumentBase
{
    /// <summary>
    /// Stát vydání dokladu
    /// </summary>
    public int? IssuingCountryId { get; set; }

    /// <summary>
    /// Doklad vydal
    /// </summary>
    public string IssuedBy { get; set; } = string.Empty;

    public DateTime? ValidTo { get; set; }

    public DateTime? IssuedOn { get; set; }

    public string? RegisterPlace { get; set; }
}