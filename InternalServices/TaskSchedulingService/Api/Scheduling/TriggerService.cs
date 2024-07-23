using CIS.Core.Data;
using NCrontab;
using NCrontab.Scheduler;
using CIS.Infrastructure.Data;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal sealed class TriggerService(
    IConnectionProvider _dbConnection, 
    ILogger<TriggerService> _logger, 
    IMediator _mediator)
{
    private const string _sql = "SELECT ScheduleTriggerId, ScheduleJobId, Cron, JobData, IsDisabled FROM dbo.ScheduleTrigger";

    public void UpdateTriggersInScheduler(IScheduler scheduler)
    {
        var allTriggers = _dbConnection
            .ExecuteDapperRawSqlToList<(Guid ScheduleTriggerId, Guid ScheduleJobId, string Cron, string? JobData, bool IsDisabled)>(_sql);

        foreach (var trigger in allTriggers)
        {
            if (trigger.IsDisabled)
            {
                _logger.TriggerIsDisabled(trigger.ScheduleTriggerId);
                continue;
            }

            try
            {
                scheduler.AddTask(trigger.ScheduleTriggerId, trigger.Cron, ct =>
                {
                    var notification = new JobRunnerNotification(Guid.NewGuid(), trigger.ScheduleJobId, trigger.ScheduleTriggerId, trigger.JobData, true);
                    _mediator.Publish(notification, CancellationToken.None);
                });
            }
            catch (CrontabException e)
            {
                _logger.InvalidCronExpression(trigger.ScheduleTriggerId, trigger.Cron, e.Message);
            }
        }
    }
}
