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

    private const string _sql = "SELECT ScheduleTriggerId, ScheduleJobId, Cron, JobData, IsDisabled FROM dbo.ScheduleTrigger";

    public TriggerService(IConnectionProvider dbConnection, ILogger<TriggerService> logger, JobExecutor jobExecutor)
    {
        _dbConnection = dbConnection;
        _logger = logger;
        _jobExecutor = jobExecutor;
    }

    public void UpdateTriggersInScheduler(IScheduler scheduler, CancellationToken cancellationToken)
    {
        var allTriggers = _dbConnection
            .ExecuteDapperRawSqlToList<(Guid ScheduleTriggerId, Guid ScheduleJobId, string Cron, string? JobData, bool IsDisabled)>(_sql);

        foreach (var trigger in allTriggers)
        {
            if (trigger.IsDisabled)
            {
                _logger.TriggerIsDisabled(trigger.ScheduleTriggerId);
                break;
            }

            try
            {
                scheduler.AddTask(trigger.ScheduleTriggerId, trigger.Cron, ct =>
                {
                    _jobExecutor.EnqueueJob(trigger.ScheduleJobId, trigger.ScheduleTriggerId, trigger.JobData, cancellationToken);
                });
            }
            catch (CrontabException e)
            {
                _logger.InvalidCronExpression(trigger.ScheduleTriggerId, trigger.Cron, e.Message);
            }
        }
    }
}
