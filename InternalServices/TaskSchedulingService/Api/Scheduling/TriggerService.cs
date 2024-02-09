using CIS.Core.Attributes;
using CIS.Core.Data;
using NCrontab;
using NCrontab.Scheduler;
using CIS.Infrastructure.Data;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal sealed class TriggerService
{
    private readonly IConnectionProvider _dbConnection;
    private readonly ILogger<TriggerService> _logger;
    private readonly JobExecutor _jobExecutor;

    public TriggerService(IConnectionProvider dbConnection, ILogger<TriggerService> logger, JobExecutor jobExecutor)
    {
        _dbConnection = dbConnection;
        _logger = logger;
        _jobExecutor = jobExecutor;
    }

    public void UpdateTriggersInScheduler(IScheduler scheduler, CancellationToken cancellationToken)
    {
        var allTriggers = _dbConnection
            .ExecuteDapperRawSqlToList<(Guid ScheduleTriggerId, Guid ScheduleJobId, string Cron, bool IsDisabled)>("SELECT ScheduleTriggerId, ScheduleJobId, Cron, IsDisabled FROM dbo.ScheduleTrigger");

        foreach (var trigger in allTriggers)
        {
            if (trigger.IsDisabled)
            {
                _logger.LogInformation("Trigger {TriggerId} is disabled", trigger.ScheduleTriggerId);
                break;
            }

            try
            {
                scheduler.AddTask(trigger.ScheduleTriggerId, trigger.Cron, ct =>
                {
                    _jobExecutor.EnqueueJob(trigger.ScheduleJobId, trigger.ScheduleTriggerId, cancellationToken);
                });
                Console.WriteLine("AD 1");
            }
            catch (CrontabException e)
            {
                _logger.LogError("Can not add trigger {TriggerId} to scheduler due to invalid Cron Expression '{Cron}': {Message}", trigger.ScheduleTriggerId, trigger.Cron, e.Message);
            }
        }
    }
}
