using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Database.Entities;

[Table("Case", Schema = "dbo")]
internal sealed class QueueRequestId
{
    [Key]
    public int RequestId { get; set; }

    public long CaseId { get; set; }

    public DateTime CreatedTime { get; set; }
}
