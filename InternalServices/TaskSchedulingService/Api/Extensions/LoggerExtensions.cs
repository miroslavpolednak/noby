namespace CIS.InternalServices.TaskSchedulingService.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, bool, string?, Exception> _tryToAcquireScheduleLock = LoggerMessage.Define<bool, string?>(
        LogLevel.Debug,
        new EventId(LoggerEventIdCodes.TryToAcquireScheduleLock, nameof(TryToAcquireScheduleLock)),
        "Acquiring instance lock with result: {IsLockAcquired}, Instance: {InstanceName}");

    private static readonly Action<ILogger, Guid, Exception> _triggerIsDisabled = LoggerMessage.Define<Guid>(
        LogLevel.Debug,
        new EventId(LoggerEventIdCodes.TriggerIsDisabled, nameof(TriggerIsDisabled)),
        "Trigger {TriggerId} is disabled");

    private static readonly Action<ILogger, Guid, string, string, Exception> _invalidCronExpression = LoggerMessage.Define<Guid, string, string>(
        LogLevel.Error,
        new EventId(LoggerEventIdCodes.InvalidCronExpression, nameof(InvalidCronExpression)),
        "Can not add trigger {TriggerId} to scheduler due to invalid Cron Expression '{Cron}': {Message}");

    private static readonly Action<ILogger, Guid, Exception> _jobNotFound = LoggerMessage.Define<Guid>(
        LogLevel.Error,
        new EventId(LoggerEventIdCodes.JobNotFound, nameof(JobNotFound)),
        "Job {JobId} not found or is disabled");

    private static readonly Action<ILogger, Guid, Guid?, Exception> _skippingTrigger = LoggerMessage.Define<Guid, Guid?>(
        LogLevel.Debug,
        new EventId(LoggerEventIdCodes.SkippingTrigger, nameof(SkippingTrigger)),
        "Current instance does not hold lock. Skipping job {JobId} for trigger {TriggerId}.");

    private static readonly Action<ILogger, Guid, Guid?, Guid, Exception> _enqueingJob = LoggerMessage.Define<Guid, Guid?, Guid>(
        LogLevel.Debug,
        new EventId(LoggerEventIdCodes.EnqueingJob, nameof(EnqueingJob)),
        "Enqueing job {JobId} for trigger {TriggerId} with statusId {StatusId}");

    private static readonly Action<ILogger, Guid, Exception> _jobFinished = LoggerMessage.Define<Guid>(
        LogLevel.Information,
        new EventId(LoggerEventIdCodes.JobFinished, nameof(JobFinished)),
        "Job {JobId} finished");

    private static readonly Action<ILogger, Guid, string, Exception> _jobFailed = LoggerMessage.Define<Guid, string>(
        LogLevel.Error,
        new EventId(LoggerEventIdCodes.JobFailed, nameof(JobFailed)),
        "Job {JobId} failed: {Message}");

    private static readonly Action<ILogger, Guid, Guid, Exception> _jobLocked = LoggerMessage.Define<Guid, Guid>(
        LogLevel.Information,
        new EventId(LoggerEventIdCodes.JobLocked, nameof(JobLocked)),
        "Job {JobId} with state {StateId} has not been executed since distributed lock already exist");

    private static readonly Action<ILogger, string, Exception> _instanceLockAcquireFailed = LoggerMessage.Define<string>(
        LogLevel.Warning,
        new EventId(LoggerEventIdCodes.InstanceLockAcquireFailed, nameof(InstanceLockAcquireFailed)),
        "Acquiring instance lock failed: {Message}");

    private static readonly Action<ILogger, Exception> _instanceUnableToAcquireLock = LoggerMessage.Define(
        LogLevel.Warning,
        new EventId(LoggerEventIdCodes.InstanceUnableToAcquireLock, nameof(InstanceUnableToAcquireLock)),
        "Acquiring instance lock failed: can't obtain lock");

    private static readonly Action<ILogger, int, Guid, Exception> _jobTimeoutReached = LoggerMessage.Define<int, Guid>(
        LogLevel.Warning,
        new EventId(LoggerEventIdCodes.JobTimeoutReached, nameof(JobTimeoutReached)),
        "Job timeout {Minutes} reached for trigger {TriggerId}");

    public static void JobTimeoutReached(this ILogger logger, in Guid triggerId, in int timeout)
        => _jobTimeoutReached(logger, timeout, triggerId, null!);

    public static void InstanceUnableToAcquireLock(this ILogger logger)
        => _instanceUnableToAcquireLock(logger, null!);

    public static void InstanceLockAcquireFailed(this ILogger logger, Exception ex)
        => _instanceLockAcquireFailed(logger, ex.Message, ex);

    public static void TryToAcquireScheduleLock(this ILogger logger, in bool isLockAcquired, in string? lockOwnerInstanceName)
        => _tryToAcquireScheduleLock(logger, isLockAcquired, lockOwnerInstanceName, null!);

    public static void TriggerIsDisabled(this ILogger logger, in Guid triggerId)
        => _triggerIsDisabled(logger, triggerId, null!);

    public static void InvalidCronExpression(this ILogger logger, in Guid triggerId, in string cron, in string message)
        => _invalidCronExpression(logger, triggerId, cron, message, null!);

    public static void JobNotFound(this ILogger logger, in Guid jobId)
        => _jobNotFound(logger, jobId, null!);

    public static void SkippingTrigger(this ILogger logger, in Guid jobId, in Guid? triggerId)
        => _skippingTrigger(logger, jobId, triggerId, null!);

    public static void EnqueingJob(this ILogger logger, Guid jobStatusId, in Guid jobId, in Guid? triggerId)
        => _enqueingJob(logger, jobId, triggerId, jobStatusId, null!);

    public static void JobFinished(this ILogger logger, in Guid jobId)
        => _jobFinished(logger, jobId, null!);

    public static void JobFailed(this ILogger logger, in Guid jobId, in string message, Exception ex)
        => _jobFailed(logger, jobId, message, ex);

    public static void JobLocked(this ILogger logger, in Guid jobId, in Guid stateId)
        => _jobLocked(logger, jobId, stateId, null!);
}
