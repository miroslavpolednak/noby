namespace CIS.Infrastructure.BackgroundServices;

internal static class BackgroundServiceLogExtensions
{
    private static readonly Action<ILogger, string, DateTime, Exception> _backgroundServiceNextRun;
    private static readonly Action<ILogger, string, Exception> _backgroundServiceIsDisabled;
    private static readonly Action<ILogger, string, Exception> _backgroundServiceExecutionError;
    private static readonly Action<ILogger, string, string, Exception> _backgroundServiceRegistered;
    private static readonly Action<ILogger, string, Exception> _parallelJobTerminated;
    private static readonly Action<ILogger, int, Exception> _dbCannotSetAppLock;

    static BackgroundServiceLogExtensions()
    {
        _backgroundServiceIsDisabled = LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(700, nameof(BackgroundServiceIsDisabled)),
            "Background service '{BackgroundServiceName}' is disabled in configuration.");

        _backgroundServiceExecutionError = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(701, nameof(BackgroundServiceExecutionError)),
            "An error occurred in background service '{BackgroundServiceName}' execution loop.");

        _backgroundServiceRegistered = LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(702, nameof(BackgroundServiceRegistered)),
            "Background service '{BackgroundServiceName}' registered with following Crontab: {Cron}.");

        _backgroundServiceNextRun = LoggerMessage.Define<string, DateTime>(
           LogLevel.Debug,
           new EventId(703, nameof(BackgroundServiceNextRun)),
           "Background service '{BackgroundServiceName}' scheduled for next run at {NextRun}");

        _parallelJobTerminated = LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(704, nameof(ParallelJobTerminated)),
            "Job '{JobName}' lock have been already acquired, parallel job gonna be terminated, or database for locking is unreachable");

        _dbCannotSetAppLock = LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(705, nameof(DbCannotSetAppLock)),
            "Database cannot set applock reason: {ReturnCode}");
    }

    public static void DbCannotSetAppLock(this ILogger logger, int returnCode)
        => _dbCannotSetAppLock(logger, returnCode, null!);

    public static void BackgroundServiceNextRun(this ILogger logger, string backgroundServiceName, DateTime nextRun)
        => _backgroundServiceNextRun(logger, backgroundServiceName, nextRun, null!);

    public static void BackgroundServiceIsDisabled(this ILogger logger, string backgroundServiceName)
        => _backgroundServiceIsDisabled(logger, backgroundServiceName, null!);

    public static void BackgroundServiceExecutionError(this ILogger logger, string backgroundServiceName, Exception ex)
        => _backgroundServiceExecutionError(logger, backgroundServiceName, ex);

    public static void BackgroundServiceRegistered(this ILogger logger, string backgroundServiceName, string cronConfiguration)
        => _backgroundServiceRegistered(logger, backgroundServiceName, cronConfiguration, null!);

    public static void ParallelJobTerminated(this ILogger logger, string jobName)
        => _parallelJobTerminated(logger, jobName, null!);
}
