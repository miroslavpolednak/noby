using CIS.Core.Exceptions;
using Medallion.Threading.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal sealed class JobRunnerHandler
    : INotificationHandler<JobRunnerNotification>
{
    public async Task Handle(JobRunnerNotification notification, CancellationToken cancellationToken)
    {
        // nejedna se o aktivni instanci sluzby
        if (notification.ActiveInstanceCheck && !_lockingService.CurrentState.IsLockAcquired)
        {
            _logger.SkippingTrigger(notification.JobId, notification.TriggerId);
            return;
        }

        // tady musi byt scope, protoze mediator.Publish je pousteny bez await, takze jeho puvodni scope skonci driv, nez tenhle handler dojede
        using (var serviceScope = _serviceScopeFactory.CreateAsyncScope())
        {
            _dbContext = serviceScope.ServiceProvider.GetRequiredService<Database.TaskSchedulingServiceDbContext>();

            // info o jobu. Chci se tady pokazde koukat do DB a nekesovat to. 
            var jobInfo = await _dbContext.ScheduleJobs
                .AsNoTracking()
                .Where(t => !t.IsDisabled && t.ScheduleJobId == notification.JobId)
                .Select(t => new { t.JobType })
                .FirstOrDefaultAsync(cancellationToken);

            if (jobInfo is null)
            {
                _logger.JobNotFound(notification.JobId);
                return;
            }

            // ziskat job type
            var jobType = _jobTypes.GetOrAdd(notification.JobId, id =>
            {
                var jobType = Type.GetType(jobInfo.JobType);
                if (jobType is null || !jobType.IsAssignableTo(typeof(IJob)))
                {
                    throw new CisArgumentException(601, $"Type '{jobInfo.JobType}' can not be created or is not IJob");
                }

                return jobType;
            });

            // vytvorit nove TraceId
            using (var activity = _activitySource.StartActivity(nameof(JobRunnerHandler)))
            {
                _logger.EnqueingJob(notification.JobStatusId, notification.JobId, notification.TriggerId);

                // nastavit novy status jobu a zapsat ho do databaze
                var statusEntity = await createStatus(notification, cancellationToken);

                _logger.EnqueingJob(notification.JobStatusId, notification.JobId, notification.TriggerId);

                // instance jobu z DI
                var job = (IJob)serviceScope.ServiceProvider.GetRequiredService(jobType);

                // run job
                await executeJob(statusEntity, job, notification, cancellationToken);
            }
        }
    }

    private async Task<Database.Entities.ScheduleJobStatus> createStatus(JobRunnerNotification notification, CancellationToken cancellationToken)
    {
        var statusEntity = new Database.Entities.ScheduleJobStatus
        {
            ScheduleJobStatusId = notification.JobStatusId,
            ScheduleJobId = notification.JobId,
            Status = ScheduleJobStatuses.InProgress.ToString(),
            ScheduleTriggerId = notification.TriggerId,
            StartedAt = _timeProvider.GetLocalNow().DateTime,
            TraceId = Activity.Current?.TraceId.ToString()
        };
        _dbContext.Add(statusEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return statusEntity;
    }

    private async Task executeJob(Database.Entities.ScheduleJobStatus statusEntity, IJob job, JobRunnerNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            var distributedLock = new SqlDistributedLock(notification.JobId.ToString(), _connectionString!);
            await using (var handle = await distributedLock.TryAcquireAsync(timeout: TimeSpan.FromSeconds(1), cancellationToken: cancellationToken))
            {
                if (handle != null)
                {
                    await job.Execute(notification.JobData, cancellationToken);

                    saveStatusChange(statusEntity, ScheduleJobStatuses.Finished);
                    _logger.JobFinished(notification.JobId);
                }
                else
                {
                    saveStatusChange(statusEntity, ScheduleJobStatuses.FailedBecauseOfLock);
                    _logger.JobLocked(notification.JobId, notification.JobStatusId);
                }
            }
        }
        catch (Exception ex)
        {
            saveStatusChange(statusEntity, ScheduleJobStatuses.Failed);
            _logger.JobFailed(notification.JobId, ex.Message, ex);
        }
    }

    private void saveStatusChange(Database.Entities.ScheduleJobStatus statusEntity, in ScheduleJobStatuses newStatus, in string? failedMessage = null)
    {
        statusEntity.Status = newStatus.ToString();
        statusEntity.FailedMessage = failedMessage;
        
        _dbContext!.SaveChanges();
    }

    private Database.TaskSchedulingServiceDbContext? _dbContext;

    private readonly TimeProvider _timeProvider;
    private readonly ILogger<JobRunnerHandler> _logger;
    private readonly InstanceLocking.ScheduleInstanceLockStatusService _lockingService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // kes Type jobu
    private static ConcurrentDictionary<Guid, Type> _jobTypes = new ConcurrentDictionary<Guid, Type>();
    // TraceId - activity
    private static ActivitySource _activitySource = new(typeof(JobRunnerHandler).Name);
    // nakesovany connection string kvuli distr locku. mrda pes na to ze neni thread safe
    private static string? _connectionString;

    // subscribe to activityu listener
    static JobRunnerHandler()
    {
        ActivitySource.AddActivityListener(new()
        {
            ShouldListenTo = _ => true,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
        });
    }

    public JobRunnerHandler(
        IConfiguration configuration,
        ILogger<JobRunnerHandler> logger,
        InstanceLocking.ScheduleInstanceLockStatusService lockingService,
        TimeProvider timeProvider,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _lockingService = lockingService;
        _timeProvider = timeProvider;
        _serviceScopeFactory = serviceScopeFactory;

        if (_connectionString == null)
        {
            _connectionString = configuration.GetConnectionString(CIS.Core.CisGlobalConstants.DefaultConnectionStringKey);
        }
    }
}
