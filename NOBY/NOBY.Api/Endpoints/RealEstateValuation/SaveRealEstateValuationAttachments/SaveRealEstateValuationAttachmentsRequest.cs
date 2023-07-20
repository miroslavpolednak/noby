using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveRealEstateValuationAttachments;

public sealed class SaveRealEstateValuationAttachmentsRequest
    : IRequest<List<SaveRealEstateValuationAttachmentsResponseItem>>
{
    [Required]
    public long CaseId { get; set; }

    [Required]
    public int RealEstateValuationId { get; set; }
    
    /// <summary>
    /// Seznam uploadnutých příloh, které se mají založit k ocenění
    /// </summary>
    [Required]
    public List<SaveRealEstateValuationAttachmentsRequestItem>? Attachments { get; set; }
}

public sealed class SaveRealEstateValuationAttachmentsRequestItem
{
    /// <summary>
    /// ID uploadovaného souboru
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid TempFileId { get; set; }

    /// <summary>
    /// Popis souboru
    /// </summary>
    /// <example>Fotka dveří</example>
    public string? Title { get; set; }
}