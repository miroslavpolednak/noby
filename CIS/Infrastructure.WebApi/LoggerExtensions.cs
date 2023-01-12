namespace CIS.Infrastructure.WebApi;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, Exception> _webApiUncoughtException;
    private static readonly Action<ILogger, string, Exception> _webApiAuthenticationException;
    private static readonly Action<ILogger, string, Exception> _webApiNotImplementedException;

    static LoggerExtensions()
    {
        _webApiAuthenticationException = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(702, nameof(WebApiAuthenticationException)),
            "ApiExceptionMiddleware authentication failed: {Message}");

        _webApiNotImplementedException = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(703, nameof(WebApiNotImplementedException)),
            "ApiExceptionMiddleware Not Implemented: {Message}");

        _webApiUncoughtException = LoggerMessage.Define(
            LogLevel.Error,
            new EventId(704, nameof(WebApiUncoughtException)),
            "Uncought exception");
    }

    public static void WebApiAuthenticationException(this ILogger logger, string message, Exception ex)
        => _webApiAuthenticationException(logger, message, ex);

    public static void WebApiNotImplementedException(this ILogger logger, string message, Exception ex)
        => _webApiNotImplementedException(logger, message, ex);

    public static void WebApiUncoughtException(this ILogger logger, Exception ex)
        => _webApiUncoughtException(logger, ex);
}