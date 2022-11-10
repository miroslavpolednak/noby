namespace NOBY.Api.SharedDto;

public class IdentificationDocumentWithIssuedBy
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
}
