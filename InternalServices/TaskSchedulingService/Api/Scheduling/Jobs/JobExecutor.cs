using System.Diagnostics;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal sealed class JobExecutor
{
    private readonly ILogger<JobExecutor> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Guid, Type> _jobs;
    private readonly TimeProvider _timeProvider;
    private readonly JobExecutorRepository _repository;
    private readonly InstanceLocking.ScheduleInstanceLockStatusService _lockingService;

    private static ActivitySource _activitySource = new(typeof(JobExecutor).Name);
    
    // aktualni stav provadeni jobu [TriggerId,]
    private Dictionary<Guid, JobStatus> _jobStatuses = new();

    static JobExecutor()
    {
        // subscribe to listener
        ActivitySource.AddActivityListener(new()
        {
            ShouldListenTo = _ => true,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
        });
    }

    private JobExecutor(
        IServiceProvider serviceProvider,
        InstanceLocking.ScheduleInstanceLockStatusService lockingService,
        TimeProvider timeProvider,
        JobExecutorRepository repository,
        ILogger<JobExecutor> logger,
        Dictionary<Guid, Type> jobs)
    {
        _serviceProvider = serviceProvider;
        _lockingService = lockingService;
        _timeProvider = timeProvider;
        _repository = repository;
        _logger = logger;
        _jobs = jobs;
    }

    public EnqueueJobResult EnqueueJob(Guid jobId, Guid? triggerId, string? jobData, CancellationToken cancellationToken)
    {
        if (!_jobs.TryGetValue(jobId, out var jobType))
        {
            _logger.JobNotFound(jobId);
            return new EnqueueJobResult($"Job {jobId} not found");
        }

        if (!_lockingService.CurrentState.IsLockAcquired)
        {
            _logger.SkippingTrigger(jobId, triggerId);
            return new EnqueueJobResult("Current instance does not hold lock.");
        }

        if (!validateJobStatus(jobId))
        {
            _logger.JobAlreadyRunning(jobId);
            return new EnqueueJobResult("Job is already running");
        }

        // DI scope + Activity scope
        using var scope = _serviceProvider.CreateScope();
        using var activity = _activitySource.StartActivity(nameof(JobExecutor));

        string? traceId = Activity.Current?.TraceId.ToString();
        // instance statusu jobu
        var status = _jobStatuses[jobId];
        // ID noveho status jobu
        var currentStateId = Guid.NewGuid();

        // nastavit novy status jobu a zapsat ho do databaze
        status.Start(_timeProvider.GetLocalNow().DateTime, currentStateId);
        _repository.JobStarted(currentStateId, jobId, triggerId, traceId);

        _logger.EnqueingJob(jobId, triggerId);

        var job = (IJob)scope.ServiceProvider.GetService(jobType)!;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        executeJob(job, jobId, currentStateId, jobData, cancellationToken);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        return new EnqueueJobResult(traceId, currentStateId);
    }

    private async Task executeJob(IJob job, Guid jobId, Guid currentStateId, string? jobData, CancellationToken cancellationToken)
    {
        try
        {
            await job.Execute(jobData, cancellationToken);
            _repository.UpdateJobState(currentStateId, ScheduleJobStatuses.Finished);

            _logger.JobFinished(jobId);
        }
        catch (Exception ex)
        {
            _repository.UpdateJobState(currentStateId, ScheduleJobStatuses.Failed);

            _logger.JobFailed(jobId, ex.Message, ex);
        }
        finally
        {
            _jobStatuses[jobId].Reset();
        }
    }

    private bool validateJobStatus(in Guid jobId)
    {
        // job jeste nema zadny status
        if (!_jobStatuses.TryGetValue(jobId, out var status))
        {
            _jobStatuses.Add(jobId, new());
        }
        // job already running
        else if (status.IsRunning && status.WillBeStaleAt > _timeProvider.GetLocalNow().DateTime)
        {
            return false;
        }
        // stale run
        else if (status.IsRunning && status.WillBeStaleAt < _timeProvider.GetLocalNow().DateTime)
        {
            _repository.UpdateJobState(status.StateId, ScheduleJobStatuses.Stale);
            status.Reset();
        }

        return true;
    }

    /// <summary>
    /// Vytvoreni nove instance executoru
    /// </summary>
    public static JobExecutor CreateInstance(IServiceProvider serviceProvider)
    {
        var repository = serviceProvider.GetRequiredService<JobExecutorRepository>();
        var lockingService = serviceProvider.GetRequiredService<InstanceLocking.ScheduleInstanceLockStatusService>();
        var timeProvider = serviceProvider.GetRequiredService<TimeProvider>();
        var logger = serviceProvider.GetRequiredService<ILogger<JobExecutor>>();

        var jobs = repository.GetActiveJobs();

        return new JobExecutor(serviceProvider, lockingService, timeProvider, repository, logger, jobs);
    }

    private sealed class JobStatus
    {
        public DateTime? WillBeStaleAt { get; private set; }
        public bool IsRunning { get; private set; }
        public Guid StateId { get; private set; }

        public void Reset()
        {
            StateId = Guid.Empty;
            IsRunning = false;
            WillBeStaleAt = null;
        }

        public void Start(DateTime currentTime, Guid stateId)
        {
            StateId = stateId;
            IsRunning = true;
            WillBeStaleAt = getStaleTime(currentTime);
        }

        private static DateTime getStaleTime(DateTime currentTime)
            => currentTime.AddSeconds(SchedulingConstants.DefaultStaleJobTimeout);
    }
}
