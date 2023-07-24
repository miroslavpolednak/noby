namespace NOBY.Dto.RealEstateValuation;

/// <summary>
/// Příloha k ocenění nemovitosti
/// </summary>
public sealed class RealEstateValuationAttachment
{
    /// <summary>
    /// ID přílohy v NOBY
    /// </summary>
    [Required]
    public int RealEstateValuationAttachmentId { get; set; }

    /// <summary>
    /// Uživatelský popis souboru
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Název souboru
    /// </summary>
    [Required]
    public string FileName { get; set; } = string.Empty;

    public int AcvAttachmentCategoryId { get; set; }

    public string AcvAttachmentCategoryName { get; set; } = string.Empty;
}
