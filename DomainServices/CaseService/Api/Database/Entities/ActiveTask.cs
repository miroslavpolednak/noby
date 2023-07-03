using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Database.Entities;

[Table("ActiveTask", Schema = "dbo")]
internal sealed class ActiveTask
{
    [Key]
    public int ActiveTaskId { get; set; }

    public long CaseId { get; set; }

    public long TaskProcessId { get; set; }

    public int TaskTypeId { get; set; }

    public int TaskIdSb { get; set; }
}
