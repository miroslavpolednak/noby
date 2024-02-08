using CIS.Core.Data;
using NCrontab;
using NCrontab.Scheduler;
using CIS.Infrastructure.Data;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal sealed class SchedulerHostedService
    : IHostedService
{
    private readonly IScheduler _scheduler;
    private readonly IMediator _mediator;
    private readonly ILogger<SchedulerHostedService> _logger;
    private readonly IConnectionProvider _dbConnection;
    
    public SchedulerHostedService(IScheduler scheduler, IMediator mediator, ILogger<SchedulerHostedService> logger, IConnectionProvider dbConnection)
    {
        _scheduler = scheduler;
        _mediator = mediator;
        _logger = logger;
        _dbConnection = dbConnection;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        //_mediator.Publish(new Jobs.Job1.Job1Notification());
        //_mediator.Publish(new Jobs.Job2.Job2Notification());
        //_mediator.Publish(new Jobs.Job1.Job1Notification());
        //_mediator.Publish(new Jobs.Job1.Job1Notification());
        buildTriggers(cancellationToken);
        Task.Run()

        _scheduler.Start(cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _scheduler.Stop();
        
        return Task.CompletedTask;
    }
    
    private void buildTriggers(CancellationToken cancellationToken)
    {
        var allTriggers = _dbConnection.ExecuteDapperRawSqlToList<Database.Entities.ScheduleTrigger>("SELECT * FROM dbo.ScheduleTrigger");

        foreach (var trigger in allTriggers)
        {
            if (trigger.IsDisabled)
            {
                _logger.LogInformation("Trigger {TriggerId} is disabled", trigger.ScheduleTriggerId);
                break;
            }

            Type? jobType;
            try
            {
                jobType = Type.GetType(trigger.JobType);

                if (jobType is null || !jobType.IsAssignableTo(typeof(INotification)))
                {
                    throw new NullReferenceException($"Type '{trigger.JobType}' can not be created or is not INotification");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Can not find type {JobType} for trigger {TriggerId}: {Message}", trigger.JobType, trigger.ScheduleTriggerId, e.Message);
                throw;
            }

            try
            {
                _scheduler.AddTask(trigger.ScheduleTriggerId, trigger.Cron, cancellation =>
                {
                    var request = Activator.CreateInstance(jobType) as INotification;
                    _mediator.Publish(request, cancellationToken);
                });
            }
            catch (CrontabException e)
            {
                _logger.LogError("Can not add trigger {TriggerId} to scheduler due to invalid Cron Expression '{Cron}': {Message}", trigger.ScheduleTriggerId, trigger.Cron, e.Message);
            }
        }
    }
}
