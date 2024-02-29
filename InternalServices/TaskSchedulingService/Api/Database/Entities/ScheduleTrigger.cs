using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS.InternalServices.TaskSchedulingService.Api.Database.Entities;

[Table("ScheduleTrigger", Schema = "dbo")]
internal sealed class ScheduleTrigger
{
    [Key]
    public Guid ScheduleTriggerId { get; set; }

    public Guid ScheduleJobId { get; set; }
    public string TriggerName { get; set; } = string.Empty;
    public string Cron { get; set; } = string.Empty;
    public string? JobData { get; set; }
    public bool IsDisabled { get; set; }

    public ScheduleJob Job { get; set; }
}
