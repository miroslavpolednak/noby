using CIS.Core.Attributes;
using CIS.Core.Data;
using NCrontab;
using NCrontab.Scheduler;
using CIS.Infrastructure.Data;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

[TransientService, SelfService]
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    _jobExecutor.EnqueueJob(trigger.ScheduleJobId, trigger.ScheduleTriggerId, cancellationToken);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
