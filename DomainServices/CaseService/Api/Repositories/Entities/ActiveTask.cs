using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Repositories.Entities;

[Table("ActiveTask", Schema = "dbo")]
internal sealed class ActiveTask
{
    [Key]
    public int ActiveTaskId { get; set; }

    public long CaseId { get; set; }

    public int TaskId { get; set; }

    public int TaskTypeId { get; set; }
}
