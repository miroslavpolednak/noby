using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS.InternalServices.TaskSchedulingService.Api.Database.Entities;

[Table("ScheduleJob", Schema = "dbo")]
internal sealed class ScheduleJob
{
    [Key]
    public Guid ScheduleJobId { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDisabled { get; set; }
}
