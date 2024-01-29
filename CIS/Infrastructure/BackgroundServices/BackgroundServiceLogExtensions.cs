namespace CIS.Infrastructure.BackgroundServices;

internal static class BackgroundServiceLogExtensions
{
    private static readonly Action<ILogger, string, DateTime, Exception> _backgroundServiceNextRun;
    private static readonly Action<ILogger, string, int, Exception> _backgroundServiceExecutionError;
    private static readonly Action<ILogger, string, string, Exception> _backgroundServiceRegistered;
    private static readonly Action<ILogger, string, int, Exception> _parallelJobTerminated;
    private static readonly Action<ILogger, string, int, Exception> _backgroundServiceTaskStarted;
    private static readonly Action<ILogger, string, int, Exception> _backgroundServiceTaskFinished;

    static BackgroundServiceLogExtensions()
    {
        _backgroundServiceExecutionError = LoggerMessage.Define<string, int>(
            LogLevel.Error,
            new EventId(701, nameof(BackgroundServiceExecutionError)),
            "An error occurred in background service '{BackgroundServiceName}' iteration {Iteration}.");

        _backgroundServiceRegistered = LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(702, nameof(BackgroundServiceRegistered)),
            "Background service '{BackgroundServiceName}' registered with following Crontab: {Cron}.");

        _backgroundServiceNextRun = LoggerMessage.Define<string, DateTime>(
           LogLevel.Debug,
           new EventId(703, nameof(BackgroundServiceNextRun)),
           "Background service '{BackgroundServiceName}' scheduled for next run at {NextRun}");

        _parallelJobTerminated = LoggerMessage.Define<string, int>(
            LogLevel.Information,
            new EventId(704, nameof(ParallelJobTerminated)),
            "Job '{BackgroundServiceName}' iteration {Iteration} lock have been already acquired, parallel job gonna be terminated, or database for locking is unreachable");

        _backgroundServiceTaskStarted = LoggerMessage.Define<string, int>(
            LogLevel.Information,
            new EventId(705, nameof(BackgroundServiceTaskStarted)),
            "Job '{BackgroundServiceName}' iteration {Iteration} started");

        _backgroundServiceTaskFinished = LoggerMessage.Define<string, int>(
            LogLevel.Information,
            new EventId(706, nameof(BackgroundServiceTaskFinished)),
            "Job '{BackgroundServiceName}' iteration {Iteration} finished");
    }

    public static void BackgroundServiceNextRun(this ILogger logger, in string backgroundServiceName, DateTime nextRun)
        => _backgroundServiceNextRun(logger, backgroundServiceName, nextRun, null!);

    public static void BackgroundServiceExecutionError(this ILogger logger, in string backgroundServiceName, in int iteration, Exception ex)
        => _backgroundServiceExecutionError(logger, backgroundServiceName, iteration, ex);

    public static void BackgroundServiceRegistered(this ILogger logger, in string backgroundServiceName, in string cronConfiguration)
        => _backgroundServiceRegistered(logger, backgroundServiceName, cronConfiguration, null!);

    public static void ParallelJobTerminated(this ILogger logger, in string backgroundServiceName, in int iteration)
        => _parallelJobTerminated(logger, backgroundServiceName, iteration, null!);

    public static void BackgroundServiceTaskStarted(this ILogger logger, in string backgroundServiceName, in int iteration)
        => _backgroundServiceTaskStarted(logger, backgroundServiceName, iteration, null!);

    public static void BackgroundServiceTaskFinished(this ILogger logger, in string backgroundServiceName, in int iteration)
        => _backgroundServiceTaskFinished(logger, backgroundServiceName, iteration, null!);
}
