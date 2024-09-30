using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

/// <summary>
/// Extension metody pro ILogger pro logování HTTP requestů a responsů.
/// </summary>
public static class RequestResponseLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _requestHandlerStarted =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerStarted, nameof(RequestHandlerStarted)),
            "Request in {HandlerName} started");

    private static readonly Action<ILogger, string, string, Exception> _httpRequestStarted =
        LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.HttpRequestStarted, nameof(HttpRequestStarted)),
            "Request {HttpMethod} {Url} started");

    private static readonly Action<ILogger, string, long, Exception> _requestHandlerStartedWithId =
        LoggerMessage.Define<string, long>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerStartedWithId, nameof(RequestHandlerStartedWithId)),
            "Request in {HandlerName} started with ID {Id}");

    private static readonly Action<ILogger, string, Exception> _requestHandlerFinished =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.RequestHandlerFinished, nameof(RequestHandlerFinished)),
            "Request in {HandlerName} finished");

    private static readonly Action<ILogger, string, string, Exception> _httpRequestPayload =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(EventIdCodes.HttpRequestPayload, nameof(HttpRequestPayload)),
            "Request Payload for {HttpMethod} {Url}");

    private static readonly Action<ILogger, string, string, int, Exception> _httpResponsePayload =
        LoggerMessage.Define<string, string, int>(
            LogLevel.Information,
            new EventId(EventIdCodes.HttpResponsePayload, nameof(HttpResponsePayload)),
            "Response Payload for {HttpMethod} {Url} with status code {StatusCode}");

    private static readonly Action<ILogger, string, string, int, Exception> _httpResponseFinished =
        LoggerMessage.Define<string, string, int>(
            LogLevel.Information,
            new EventId(EventIdCodes.HttpResponseFinished, nameof(HttpResponsePayload)),
            "Response for {HttpMethod} {Url} with status code {StatusCode}");

    private static readonly Action<ILogger, string, Exception> _requestHandlerFinishedWithEmptyResult =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(592, nameof(RequestHandlerFinishedWithEmptyResult)),
            "Request in {HandlerName} finished with empty result");

    private static readonly Action<ILogger, string, string, Exception> _soapRequestPayload =
        LoggerMessage.Define<string, string>(
          LogLevel.Information,
          new EventId(EventIdCodes.SoapRequestPayload, nameof(SoapRequestPayload)),
          "Soap request {SoapMethod} {Url} started");

    private static readonly Action<ILogger, string, Exception> _soapResponsePayload =
        LoggerMessage.Define<string>(
          LogLevel.Information,
          new EventId(EventIdCodes.SoapResponsePayload, nameof(SoapResponsePayload)),
          "Soap response {Url} finished");

    public static void RequestHandlerFinishedWithEmptyResult(this ILogger logger, string handlerName)
        => _requestHandlerFinishedWithEmptyResult(logger, handlerName, null!);

    public static void HttpRequestPayload(this ILogger logger, string httpMethod, string url)
        => _httpRequestPayload(logger, httpMethod, url, null!);

    public static void HttpRequestPayload(this ILogger logger, HttpRequestMessage request)
        => _httpRequestPayload(logger, request.Method.ToString(), request.RequestUri!.ToString(), null!);
    
    public static void SoapRequestPayload(this ILogger logger, string soapMethod, string url)
        => _soapRequestPayload(logger, soapMethod, url, null!);

    public static void SoapResponsePayload(this ILogger logger, string url)
        => _soapResponsePayload(logger, url, null!);

    public static void HttpRequestStarted(this ILogger logger, HttpRequestMessage request)
        => _httpRequestStarted(logger, request.Method.ToString(), request.RequestUri!.ToString(), null!);

    public static void HttpResponsePayload(this ILogger logger, string httpMethod, string url, int statusCode)
        => _httpResponsePayload(logger, httpMethod, url, statusCode, null!);

    public static void HttpResponsePayload(this ILogger logger, HttpRequestMessage request, int statusCode)
        => _httpResponsePayload(logger, request.Method.ToString(), request.RequestUri!.ToString(), statusCode, null!);

    public static void HttpResponseFinished(this ILogger logger, HttpRequestMessage request, int statusCode)
        => _httpResponseFinished(logger, request.Method.ToString(), request.RequestUri!.ToString(), statusCode, null!);

    public static void RequestHandlerStarted(this ILogger logger, string handlerName)
        => _requestHandlerStarted(logger, handlerName, null!);

    public static void RequestHandlerStartedWithId(this ILogger logger, string handlerName, long id)
        => _requestHandlerStartedWithId(logger, handlerName, id, null!);

    public static void RequestHandlerStartedWithId(this ILogger logger, string handlerName, int id)
        => _requestHandlerStartedWithId(logger, handlerName, id, null!);

    public static void RequestHandlerFinished(this ILogger logger, string handlerName)
    => _requestHandlerFinished(logger, handlerName, null!);
}
