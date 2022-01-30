using Microsoft.Extensions.Logging;

namespace DomainServices.CaseService.Abstraction;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _requestHandlerStarted;
    private static readonly Action<ILogger, string, long, Exception> _requestHandlerStartedWithId;
    private static readonly Action<ILogger, Exception> _serviceUnavailable;
    private static readonly Action<ILogger, Exception> _extServiceUnavailable;
    private static readonly Action<ILogger, string, string, Exception> _generalException;
    private static readonly Action<ILogger, int, string, string, Exception> _invalidArgument;

    static LoggerExtensions()
    {
        _requestHandlerStarted = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(13601, nameof(RequestHandlerStarted)),
            "Request in {HandlerName} started");

        _requestHandlerStartedWithId = LoggerMessage.Define<string, long>(
            LogLevel.Debug,
            new EventId(13602, nameof(RequestHandlerStartedWithId)),
            "Request in {HandlerName} started with ID {Id}");

        _serviceUnavailable = LoggerMessage.Define(
            LogLevel.Error,
            new EventId(13603, nameof(ServiceUnavailable)),
            "CaseService unavailable");

        _extServiceUnavailable = LoggerMessage.Define(
            LogLevel.Error,
            new EventId(13604, nameof(ExtServiceUnavailable)),
            "Some of underlying services are not available or failed to call");

        _generalException = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            new EventId(13605, nameof(GeneralException)),
            "Uncought Exception in {MethodName}: {Message}");

        _invalidArgument = LoggerMessage.Define<int, string, string>(
            LogLevel.Debug,
            new EventId(13606, nameof(GeneralException)),
            "{Code} in {ArgumentName}: {Message}");
    }

    public static void RequestHandlerStarted(this ILogger logger, string handlerName)
        => _requestHandlerStarted(logger, handlerName, null!);

    public static void RequestHandlerStartedWithId(this ILogger logger, string handlerName, long id)
        => _requestHandlerStartedWithId(logger, handlerName, id, null!);

    public static void ServiceUnavailable(this ILogger logger, Exception ex)
        => _serviceUnavailable(logger, ex);

    public static void ExtServiceUnavailable(this ILogger logger, Exception ex)
        => _extServiceUnavailable(logger, ex);

    public static void GeneralException(this ILogger logger, string methodName, string message, Exception ex)
        => _generalException(logger, methodName, message, ex);

    public static void InvalidArgument(this ILogger logger, int code, string argumentName, string message, Exception ex)
        => _invalidArgument(logger, code, argumentName, message, ex);
}
