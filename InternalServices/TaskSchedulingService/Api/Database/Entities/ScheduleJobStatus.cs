using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS.InternalServices.TaskSchedulingService.Api.Database.Entities;

[Table("ScheduleJobStatus", Schema = "dbo")]
internal sealed class ScheduleJobStatus
{
    [Key]
    public Guid ScheduleJobStatusId { get; set; }
    public Guid ScheduleJobId { get; set; }
    public Guid ScheduleTriggerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? StatusChangedAt { get; set; }
    public string? TraceId { get; set; }
    public string ExecutorType { get; set; } = null!;
}
