using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.SharedDto;

public sealed class IdentificationDocumentFull
    : IdentificationDocumentBase
{
    /// <summary>
    /// Stát vydání dokladu
    /// </summary>
    [Required]
    public int IssuingCountryId { get; set; }

    /// <summary>
    /// Doklad vydal
    /// </summary>
    [Required]
    public string IssuedBy { get; set; } = string.Empty;

    [Required]
    public DateTime ValidTo { get; set; }

    [Required]
    public DateTime IssuedOn { get; set; }

    public string? RegisterPlace { get; set; }

    public CustomerIdentificationObject? CustomerIdentification { get; set; }
}