namespace NOBY.Api.Endpoints.RealEstateValuation.SaveRealEstateValuationAttachments;

public sealed class SaveRealEstateValuationAttachmentsResponseItem
{
    /// <summary>
    /// ID uploadnutého dočasného souboru
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid TempFileId { get; set; }

    /// <summary>
    /// ID nově vytvořené přílohy ocenění
    /// </summary>
    /// <example>97355</example>
    public int RealEstateValuationAttachmentId { get; set; }
}
