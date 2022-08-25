using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class RequestLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _requestHandlerStarted;
    private static readonly Action<ILogger, string, object?, Exception> _requestHandlerStartedWithRequest;
    private static readonly Action<ILogger, string, long, Exception> _requestHandlerStartedWithId;
    private static readonly Action<ILogger, string, Exception> _requestHandlerFinished;
    private static readonly Action<ILogger, string, string, Exception> _httpRequestPayload;
    private static readonly Action<ILogger, string, string, int, Exception> _httpResponsePayload;

    static RequestLoggerExtensions()
    {
        _httpRequestPayload = LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(EventIdCodes.HttpRequestPayload, nameof(HttpRequestPayload)),
            "Request Payload for {HttpMethod} {Url}");

        _httpResponsePayload = LoggerMessage.Define<string, string, int>(
            LogLevel.Information,
            new EventId(EventIdCodes.HttpResponsePayload, nameof(HttpResponsePayload)),
            "Reponse Payload for {HttpMethod} {Url} with status code {StatusCode}");

        _requestHandlerStarted = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerStarted, nameof(RequestHandlerStarted)),
            "Request in {HandlerName} started");

        _requestHandlerStartedWithId = LoggerMessage.Define<string, long>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerStartedWithId, nameof(RequestHandlerStartedWithId)),
            "Request in {HandlerName} started with ID {Id}");

        _requestHandlerStartedWithRequest = LoggerMessage.Define<string, object?>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerStartedWithRequest, nameof(RequestHandlerStartedWithId)),
            "Request in {HandlerName} started with {@Request}");

        _requestHandlerFinished = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerFinished, nameof(RequestHandlerFinished)),
            "Request in {HandlerName} finished");
    }

    public static void HttpRequestPayload(this ILogger logger, string httpMethod, string url)
        => _httpRequestPayload(logger, httpMethod, url, null!);

    public static void HttpRequestPayload(this ILogger logger, HttpRequestMessage request)
        => _httpRequestPayload(logger, request.Method.ToString(), request.RequestUri!.ToString(), null!);

    public static void HttpResponsePayload(this ILogger logger, string httpMethod, string url, int statusCode)
        => _httpResponsePayload(logger, httpMethod, url, statusCode, null!);

    public static void HttpResponsePayload(this ILogger logger, HttpRequestMessage request, int statusCode)
        => _httpResponsePayload(logger, request.Method.ToString(), request.RequestUri!.ToString(), statusCode, null!);

    public static void RequestHandlerStarted(this ILogger logger, string handlerName)
        => _requestHandlerStarted(logger, handlerName, null!);

    public static void RequestHandlerStarted(this ILogger logger, string handlerName, object? request)
        => _requestHandlerStartedWithRequest(logger, handlerName, request, null!);

    public static void RequestHandlerStartedWithId(this ILogger logger, string handlerName, long id)
        => _requestHandlerStartedWithId(logger, handlerName, id, null!);

    public static void RequestHandlerStartedWithId(this ILogger logger, string handlerName, int id)
        => _requestHandlerStartedWithId(logger, handlerName, id, null!);

    public static void RequestHandlerFinished(this ILogger logger, string handlerName)
    => _requestHandlerFinished(logger, handlerName, null!);
}
