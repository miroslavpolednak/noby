using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Telemetry;

internal static class LogEventsExtensions
{
    private static readonly Action<ILogger, string, object?, Exception> _requestHandlerStartedWithRequest;
    private static readonly Action<ILogger, string, object?, Exception> _requestHandlerFinishedWithResponse;

    static LogEventsExtensions()
    {
        _requestHandlerStartedWithRequest = LoggerMessage.Define<string, object?>(
            LogLevel.Debug,
            new EventId(590, nameof(RequestHandlerStarted)),
            "Request in {HandlerName} started with {@Request}");

        _requestHandlerFinishedWithResponse = LoggerMessage.Define<string, object?>(
            LogLevel.Debug,
            new EventId(591, nameof(RequestHandlerFinished)),
            "Request in {HandlerName} finished with {@Response}");
    }

    public static void RequestHandlerStarted(this ILogger logger, string handlerName, object? request)
        => _requestHandlerStartedWithRequest(logger, handlerName, request, null!);

    public static void RequestHandlerFinished(this ILogger logger, string handlerName, object? response)
        => _requestHandlerFinishedWithResponse(logger, handlerName, response, null!);
}
