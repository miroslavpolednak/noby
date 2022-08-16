using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Telemetry;

internal static class LogEventsExtensions
{
    private static readonly Action<ILogger, string, Exception> _requestHandlerStartedWithRequest;
    private static readonly Action<ILogger, string, Exception> _requestHandlerFinished;
    private static readonly Action<ILogger, string, Exception> _requestHandlerFinishedWithEmptyResult;

    static LogEventsExtensions()
    {
        _requestHandlerStartedWithRequest = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(590, nameof(RequestHandlerStarted)),
            "Request in {HandlerName} started");

        _requestHandlerFinished = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(591, nameof(RequestHandlerFinished)),
            "Request in {HandlerName} finished");

        _requestHandlerFinishedWithEmptyResult = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(592, nameof(RequestHandlerFinishedWithEmptyResult)),
            "Request in {HandlerName} finished with empty result");
    }

    public static void RequestHandlerStarted(this ILogger logger, string handlerName)
        => _requestHandlerStartedWithRequest(logger, handlerName, null!);

    public static void RequestHandlerFinished(this ILogger logger, string handlerName)
        => _requestHandlerFinished(logger, handlerName, null!);

    public static void RequestHandlerFinishedWithEmptyResult(this ILogger logger, string handlerName)
        => _requestHandlerFinishedWithEmptyResult(logger, handlerName, null!);
}
