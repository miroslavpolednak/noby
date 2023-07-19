namespace NOBY.Dto.RealEstateValuation;

/// <summary>
/// Identifikační údaje nemovitosti k Ocenění (bez Noby ID)
/// </summary>
public class DeedOfOwnershipDocument
{
    /// <summary>
    /// ID staženého dokumentu listu vlastnictví (LV)
    /// </summary>
    [Required]
    public long ExternalDeedOfOwnershipDocumentId { get; set; }

    /// <summary>
    /// KATUZ ID, pětimístné číslo katastrálního území
    /// </summary>
    [Required]
    public int KatuzId { get; set; }

    /// <summary>
    /// KATUZ, název katastrálního území, RUIAN katastrální území - AddressWhispererBEService|cadastralArea
    /// </summary>
    public string? KatuzTitle { get; set; }

    /// <summary>
    /// ISKN ID listu vlastnictví (LV), technický identifikátor katastru
    /// </summary>
    [Required]
    public long DeedOfOwnershipId { get; set; }

    /// <summary>
    /// Číslo listu vlastnictví (LV)
    /// </summary>
    [Required]
    public int DeedOfOwnershipNumber { get; set; }

    /// <summary>
    /// Adresa nemovitosti
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Unikátní ID nemovitosti ze systému CREM
    /// </summary>
    [Required]
    public List<long>? RealEstateIds { get; set; }
}
