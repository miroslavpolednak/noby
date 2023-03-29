using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class RollbackLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _rollbackHandlerStarted;
    private static readonly Action<ILogger, string, object, Exception> _rollbackHandlerStepDone;

    static RollbackLoggerExtensions()
    {
        _rollbackHandlerStarted = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(EventIdCodes.RollbackHandlerStarted, nameof(RollbackHandlerStarted)),
            "Rollback handler '{HandlerName}' started");

        _rollbackHandlerStepDone = LoggerMessage.Define<string, object>(
            LogLevel.Warning,
            new EventId(EventIdCodes.RollbackHandlerStepDone, nameof(RollbackHandlerStepDone)),
            "Rollback step key '{Key}' with value '{Value}' finished");
    }

    /// <summary>
    /// Spuštění rollabck handleru
    /// </summary>
    public static void RollbackHandlerStarted(this ILogger logger, string handlerName)
        => _rollbackHandlerStarted(logger, handlerName, null!);

    /// <summary>
    /// Úspěšné projetí jednoho kroku v rollback handleru
    /// </summary>
    public static void RollbackHandlerStepDone(this ILogger logger, string key, object value)
        => _rollbackHandlerStepDone(logger, key, value, null!);
}
