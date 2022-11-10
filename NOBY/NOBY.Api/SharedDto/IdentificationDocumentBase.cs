namespace NOBY.Api.SharedDto;

public class IdentificationDocumentBase
{
    /// <summary>
    /// Typ osobního dokladu - číselník IdentificationDocumentType - (CIS_TYPY_DOKLADOV)
    /// </summary>
    public int IdentificationDocumentTypeId { get; set; }

    /// <summary>
    /// Číslo osobního dokladu
    /// </summary>
    public string Number { get; set; } = String.Empty;
}
