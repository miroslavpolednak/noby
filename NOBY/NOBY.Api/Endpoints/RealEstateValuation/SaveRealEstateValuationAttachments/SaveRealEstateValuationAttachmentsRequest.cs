using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveRealEstateValuationAttachments;

public sealed class SaveRealEstateValuationAttachmentsRequest
    : IRequest<List<RealEstateValuationSaveRealEstateValuationAttachmentsResponseItem>>
{
    [Required]
    public long CaseId { get; set; }

    [Required]
    public int RealEstateValuationId { get; set; }
    
    /// <summary>
    /// Seznam uploadnutých příloh, které se mají založit k ocenění
    /// </summary>
    [Required]
    public List<RealEstateValuationSaveRealEstateValuationAttachmentsRequestItem>? Attachments { get; set; }
}
