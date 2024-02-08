using CIS.Core.Data;
using System.Diagnostics;
using CIS.Infrastructure.Data;

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
    private Dictionary<Guid, JobStatus> _jobStatuses = new();

    private JobExecutor(IServiceProvider serviceProvider, Dictionary<Guid, Type> jobs)
    {
        _serviceProvider = serviceProvider;

        _lockingService = _serviceProvider.GetRequiredService<InstanceLocking.ScheduleInstanceLockStatusService>();
        _timeProvider = _serviceProvider.GetRequiredService<TimeProvider>();
        _repository = _serviceProvider.GetRequiredService<JobExecutorRepository>();
        _logger = _serviceProvider.GetRequiredService<ILogger<JobExecutor>>();

        // subscribe to listener
        ActivitySource.AddActivityListener(new()
        {
            ShouldListenTo = _ => true,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
        });
        _jobs = jobs;
    }

    public async Task EnqueueJob(Guid jobId, Guid triggerId, CancellationToken cancellationToken)
    {
        if (!_lockingService.CurrentState.IsLockAcquired)
        {
            _logger.LogInformation("Current instance does not hold lock. Skipping job {JobId} for trigger {TriggerId}.", jobId, triggerId);
            return;
        }

        if (!_jobs.TryGetValue(jobId, out var jobType))
        {
            throw new Exception($"Job {jobId} not found");
        }

        if (!validateJobStatus(jobId))
        {
            _logger.LogInformation("Job {JobId} is already running", jobId);
            return;
        }

        // DI scope + Activity scope
        using var scope = _serviceProvider.CreateScope();
        using var activity = _activitySource.StartActivity(nameof(JobExecutor));

        // instance statusu jobu
        var status = _jobStatuses[jobId];
        // ID noveho status jobu
        var currentStateId = Guid.NewGuid();

        // nastavit novy status jobu a zapsat ho do databaze
        status.Start(_timeProvider.GetLocalNow().DateTime, currentStateId);
        _repository.JobStarted(currentStateId, jobId, triggerId, Activity.Current?.TraceId.ToString());

        _logger.LogInformation("Enqueueing job {JobId} for trigger {TriggerId}", jobId, triggerId);

        try
        {
            var job = (IJob)scope.ServiceProvider.GetService(jobType)!;
            await job.Execute(cancellationToken);
            _repository.UpdateJobState(currentStateId, JobExecutorRepository.Statuses.Finished);

            _logger.LogInformation("Job {JobId} for trigger {TriggerId} finished", jobId, triggerId);
        }
        catch (Exception ex)
        {
            _repository.UpdateJobState(currentStateId, JobExecutorRepository.Statuses.Failed);
            _logger.LogInformation("Job {JobId} for trigger {TriggerId} failed: {Message}", jobId, triggerId, ex.Message);
        }
        finally
        {
            status.Reset();
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
            _repository.UpdateJobState(status.StateId, JobExecutorRepository.Statuses.Stale);
            status.Reset();
        }

        return true;
    }

    public static JobExecutor CreateInstance(IServiceProvider serviceProvider)
    {
        var dbConnection = serviceProvider.GetRequiredService<IConnectionProvider>();
        var jobs = dbConnection
            .ExecuteDapperRawSqlToList<(Guid ScheduleJobId, string JobType)>("SELECT ScheduleJobId, JobType FROM dbo.ScheduleJob WHERE IsDisabled=0")
            .ToDictionary(k => k.ScheduleJobId, v =>
            {
                var jobType = Type.GetType(v.JobType);

                if (jobType is null || !jobType.IsAssignableTo(typeof(IJob)))
                {
                    throw new NullReferenceException($"Type '{v}' can not be created or is not IJob");
                }

                return jobType;
            });

        return new JobExecutor(serviceProvider, jobs);
    }

    private class JobStatus
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
