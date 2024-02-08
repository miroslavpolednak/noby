using CIS.Core.Data;
using NCrontab;
using NCrontab.Scheduler;
using CIS.Infrastructure.Data;
using CIS.InternalServices.TaskSchedulingService.Api.Database.Entities;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal sealed class SchedulerHostedService
    : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IScheduler _scheduler;
    private readonly ILogger<SchedulerHostedService> _logger;
    private readonly IConnectionProvider _dbConnection;

    private Dictionary<Guid, Type>? _jobTypesCache;

    public SchedulerHostedService(IScheduler scheduler, ILogger<SchedulerHostedService> logger, IConnectionProvider dbConnection, IServiceProvider serviceProvider)
    {
        _scheduler = scheduler;
        _logger = logger;
        _dbConnection = dbConnection;
        _serviceProvider = serviceProvider;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        buildJobTypes();
        buildTriggers(cancellationToken);
        
        _scheduler.Start(cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _scheduler.Stop();
        
        return Task.CompletedTask;
    }

    private void buildJobTypes()
    {
        var allTriggers = _dbConnection.ExecuteDapperRawSqlToList<(Guid ScheduleJobId, string JobType)> ("SELECT ScheduleJobId, JobType FROM dbo.ScheduleJob WHERE IsDisabled=0");
        
        _jobTypesCache = allTriggers
            .ToDictionary(k => k.ScheduleJobId, v =>
            {
                var jobType = Type.GetType(v.JobType);

                if (jobType is null || !jobType.IsAssignableTo(typeof(IJob)))
                {
                    throw new NullReferenceException($"Type '{v}' can not be created or is not IJob");
                }

                return jobType;
            });
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

            if (!_jobTypesCache!.TryGetValue(trigger.ScheduleJobId, out Type? jobType))
            {
                throw new NullReferenceException($"Job '{trigger.ScheduleJobId}' not found");
            }

            try
            {
                _scheduler.AddTask(trigger.ScheduleTriggerId, trigger.Cron, ct =>
                {
                    (new JobExecutor(_serviceProvider)).EnqueueJob(jobType, trigger.ScheduleTriggerId, cancellationToken);
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
