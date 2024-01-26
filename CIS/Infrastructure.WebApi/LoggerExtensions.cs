namespace CIS.Infrastructure.WebApi;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, Exception> _webApiUncoughtException;
    private static readonly Action<ILogger, string, Exception> _webApiAuthenticationException;
    private static readonly Action<ILogger, string, Exception> _webApiNotImplementedException;
    private static readonly Action<ILogger, string, Exception> _webApiAuthorizationException;
    private static readonly Action<ILogger, string, Exception> _notFoundException;
    private static readonly Action<ILogger, string, Exception> _nobyValidationException;

    static LoggerExtensions()
    {
        _webApiAuthenticationException = LoggerMessage.Define<string>(
              LogLevel.Warning,
              new EventId(702, nameof(WebApiAuthenticationException)),
              "Authentication failed: {Message}");

        _webApiAuthorizationException = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(705, nameof(WebApiAuthorizationException)),
            "Authorization exception: {Message}");

        _webApiNotImplementedException = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(703, nameof(WebApiNotImplementedException)),
            "ApiExceptionMiddleware Not Implemented: {Message}");

        _webApiUncoughtException = LoggerMessage.Define(
            LogLevel.Error,
            new EventId(704, nameof(WebApiUncoughtException)),
            "Uncought exception");

        _notFoundException = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(706, nameof(WebApiAuthorizationException)),
            "NotFound exception: {Message}");

        _nobyValidationException = LoggerMessage.Define<string>(
           LogLevel.Warning,
           new EventId(707, nameof(NobyValidationException)),
           "Noby validation exception: {Message}");
    }

    public static void NobyValidationException(this ILogger logger, string message, Exception ex)
         => _nobyValidationException(logger, message, ex);

    public static void NotFoundException(this ILogger logger, string message, Exception ex)
        => _notFoundException(logger, message, ex);

    public static void WebApiAuthenticationException(this ILogger logger, string message, Exception ex)
        => _webApiAuthenticationException(logger, message, ex);

    public static void WebApiAuthorizationException(this ILogger logger, string message, Exception ex)
        => _webApiAuthorizationException(logger, message, ex);

    public static void WebApiNotImplementedException(this ILogger logger, string message, Exception ex)
        => _webApiNotImplementedException(logger, message, ex);

    public static void WebApiUncoughtException(this ILogger logger, Exception ex)
        => _webApiUncoughtException(logger, ex);
}