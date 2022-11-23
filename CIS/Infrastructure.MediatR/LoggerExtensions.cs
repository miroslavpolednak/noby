using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.MediatR;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, string, Exception> _rollbackStarted;
    private static readonly Action<ILogger, Exception> _rollbackFinished;
    private static Func<ILogger, object, IDisposable?> _rollbackScope;

    static LoggerExtensions()
    {
        _rollbackStarted = LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(580, nameof(RollbackStarted)),
            "Handler Rollback started for {RequestName} because of {Exception}");

        _rollbackFinished = LoggerMessage.Define(
            LogLevel.Information,
            new EventId(581, nameof(RollbackFinished)),
            "Handler Rollback finished");

        _rollbackScope = LoggerMessage.DefineScope<object>("Request: {Request}");
    }

    public static void RollbackStarted<TRequest>(this ILogger logger, TRequest request, Exception ex)
    {
        using (IDisposable? scope = _rollbackScope(logger, request!))
        {
            _rollbackStarted(logger, nameof(request), ex.Message, ex);
        }
    }

    public static void RollbackFinished(this ILogger logger)
        => _rollbackFinished(logger, null!);
}