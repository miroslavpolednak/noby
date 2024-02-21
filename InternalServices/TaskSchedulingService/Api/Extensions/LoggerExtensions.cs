namespace CIS.InternalServices.TaskSchedulingService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, bool, string?, Exception> _tryToAcquireScheduleLock;
    private static readonly Action<ILogger, Guid, Exception> _triggerIsDisabled;
    private static readonly Action<ILogger, Guid, string, string, Exception> _invalidCronExpression;
    private static readonly Action<ILogger, Guid, Exception> _jobNotFound;
    private static readonly Action<ILogger, Guid, Guid?, Exception> _skippingTrigger;
    private static readonly Action<ILogger, Guid, Exception> _jobAlreadyRunning;
    private static readonly Action<ILogger, Guid, Guid?, Exception> _enqueingJob;
    private static readonly Action<ILogger, Guid, Exception> _jobFinished;
    private static readonly Action<ILogger, Guid, string, Exception> _jobFailed;
    private static readonly Action<ILogger, Guid, Guid, Exception> _jobLocked;
    private static readonly Action<ILogger, string, Exception> _instanceLockAcquireFailed;
    private static readonly Action<ILogger, Exception> _instanceUnableToAcquireLock;

    static LoggerExtensions()
    {
        _instanceLockAcquireFailed = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.InstanceLockAcquireFailed, nameof(InstanceLockAcquireFailed)),
            "Acquiring instance lock failed: {Message}");

        _instanceUnableToAcquireLock = LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.InstanceUnableToAcquireLock, nameof(InstanceUnableToAcquireLock)),
            "Acquiring instance lock failed: can't obtain lock");

        _tryToAcquireScheduleLock = LoggerMessage.Define<bool, string?>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.TryToAcquireScheduleLock, nameof(TryToAcquireScheduleLock)),
            "Acquiring instance lock with result: {IsLockAcquired}, Instance: {InstanceName}");

        _triggerIsDisabled = LoggerMessage.Define<Guid>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.TriggerIsDisabled, nameof(TriggerIsDisabled)),
            "Trigger {TriggerId} is disabled");

        _invalidCronExpression = LoggerMessage.Define<Guid, string, string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.InvalidCronExpression, nameof(InvalidCronExpression)),
            "Can not add trigger {TriggerId} to scheduler due to invalid Cron Expression '{Cron}': {Message}");

        _jobNotFound = LoggerMessage.Define<Guid>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.JobNotFound, nameof(JobNotFound)),
            "Job {JobId} not found");

        _skippingTrigger = LoggerMessage.Define<Guid, Guid?>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.SkippingTrigger, nameof(SkippingTrigger)),
            "Current instance does not hold lock. Skipping job {JobId} for trigger {TriggerId}.");

        _jobAlreadyRunning = LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(LoggerEventIdCodes.JobAlreadyRunning, nameof(JobAlreadyRunning)),
            "Job {JobId} is already running");

        _enqueingJob = LoggerMessage.Define<Guid, Guid?>(
            LogLevel.Debug,
            new EventId(LoggerEventIdCodes.EnqueingJob, nameof(EnqueingJob)),
            "Enqueing job {JobId} for trigger {TriggerId}");

        _jobFinished = LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.JobFinished, nameof(JobFinished)),
            "Job {JobId} finished");

        _jobFailed = LoggerMessage.Define<Guid, string>(
            LogLevel.Error,
            new EventId(LoggerEventIdCodes.JobFailed, nameof(JobFailed)),
            "Job {JobId} failed: {Message}");

        _jobLocked = LoggerMessage.Define<Guid, Guid>(
            LogLevel.Information,
            new EventId(LoggerEventIdCodes.JobLocked, nameof(JobLocked)),
            "Job {JobId} with state {StateId} has not been executed since distributed lock already exist");
    }

    public static void InstanceUnableToAcquireLock(this ILogger logger)
        => _instanceUnableToAcquireLock(logger, null!);

    public static void InstanceLockAcquireFailed(this ILogger logger, Exception ex)
        => _instanceLockAcquireFailed(logger, ex.Message, ex);

    public static void TryToAcquireScheduleLock(this ILogger logger, bool IsLockAcquired, string? lockOwnerInstanceName)
        => _tryToAcquireScheduleLock(logger, IsLockAcquired, lockOwnerInstanceName, null!);

    public static void TriggerIsDisabled(this ILogger logger, Guid triggerId)
        => _triggerIsDisabled(logger, triggerId, null!);

    public static void InvalidCronExpression(this ILogger logger, Guid triggerId, string cron, string message)
        => _invalidCronExpression(logger, triggerId, cron, message, null!);

    public static void JobNotFound(this ILogger logger, Guid jobId)
        => _jobNotFound(logger, jobId, null!);

    public static void SkippingTrigger(this ILogger logger, Guid jobId, Guid? triggerId)
        => _skippingTrigger(logger, jobId, triggerId, null!);

    public static void JobAlreadyRunning(this ILogger logger, Guid jobId)
        => _jobAlreadyRunning(logger, jobId, null!);

    public static void EnqueingJob(this ILogger logger, Guid jobId, Guid? triggerId)
        => _enqueingJob(logger, jobId, triggerId, null!);

    public static void JobFinished(this ILogger logger, Guid jobId)
        => _jobFinished(logger, jobId, null!);

    public static void JobFailed(this ILogger logger, Guid jobId, string message, Exception ex)
        => _jobFailed(logger, jobId, message, ex);

    public static void JobLocked(this ILogger logger, Guid jobId, Guid stateId)
        => _jobLocked(logger, jobId, stateId, null!);
}
