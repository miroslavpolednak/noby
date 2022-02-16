using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class RequestLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _requestHandlerStarted;
    private static readonly Action<ILogger, string, long, Exception> _requestHandlerStartedWithId;
    private static readonly Action<ILogger, string, Exception> _requestHandlerFinished;

    static RequestLoggerExtensions()
    {
        _requestHandlerStarted = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerStarted, nameof(RequestHandlerStarted)),
            "Request in {HandlerName} started");

        _requestHandlerStartedWithId = LoggerMessage.Define<string, long>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerStartedWithId, nameof(RequestHandlerStartedWithId)),
            "Request in {HandlerName} started with ID {Id}");

        _requestHandlerFinished = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerFinished, nameof(RequestHandlerFinished)),
            "Request in {HandlerName} finished");
    }

    public static void RequestHandlerStarted(this ILogger logger, string handlerName)
        => _requestHandlerStarted(logger, handlerName, null!);

    public static void RequestHandlerStartedWithId(this ILogger logger, string handlerName, long id)
        => _requestHandlerStartedWithId(logger, handlerName, id, null!);

    public static void RequestHandlerFinished(this ILogger logger, string handlerName)
    => _requestHandlerFinished(logger, handlerName, null!);
}
