using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS.InternalServices.TaskSchedulingService.Api.Database.Entities;

[Table("ScheduleTrigger", Schema = "dbo")]
internal sealed class ScheduleTrigger
{
    [Key]
    public Guid ScheduleTriggerId { get; set; }

    public string TriggerName { get; set; } = null!;

    public string Cron { get; set; } = null!;

    public string JobType { get; set; } = null!;

    public bool IsDisabled { get; set; }
}
