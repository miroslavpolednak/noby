using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Repositories.Entities;

[Table("CaseActiveTasks", Schema = "dbo")]
internal class CaseActiveTasks
{
    [Key]
    public int CaseActiveTasksId { get; set; }

    public long CaseId { get; set; }

    public string? ActiveTasks { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Case ParentCase { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
