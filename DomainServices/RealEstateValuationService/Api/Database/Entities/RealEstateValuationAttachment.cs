using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.RealEstateValuationService.Api.Database.Entities;

[Table("RealEstateValuationAttachment", Schema = "dbo")]
internal sealed class RealEstateValuationAttachment
    : CIS.Core.Data.BaseCreated
{
    [Key]
    public int RealEstateValuationAttachmentId { get; set; }

    public int RealEstateValuationId { get; set; }
    public long ExternalId { get; set; }
    public string? Title { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int AcvAttachmentCategoryId { get; set; }
}
