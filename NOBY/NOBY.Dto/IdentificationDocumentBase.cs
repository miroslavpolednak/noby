namespace NOBY.Dto;

public class IdentificationDocumentBase
{
    /// <summary>
    /// Typ osobního dokladu - číselník IdentificationDocumentType - (CIS_TYPY_DOKLADOV)
    /// </summary>
    [Required]
    public int? IdentificationDocumentTypeId { get; set; }

    /// <summary>
    /// Číslo osobního dokladu
    /// </summary>
    [Required]
    public string Number { get; set; } = string.Empty;
}
