using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

/// <summary>
/// Extension metody pro ILogger v týkající se handlingu webových služeb třetích stran.
/// </summary>
public static class ServiceLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _serviceUnavailable;
    private static readonly Action<ILogger, Exception> _serviceAuthorizationFailed;
    private static readonly Action<ILogger, string, Exception> _extServiceUnavailable;
    private static readonly Action<ILogger, Exception> _extServiceAuthenticationFailed;
    private static readonly Action<ILogger, string, Exception> _extServiceResponseError;
    private static readonly Action<ILogger, string, Exception> _extServiceServerError;

    static ServiceLoggerExtensions()
    {
        _serviceUnavailable = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(EventIdCodes.ServiceUnavailable, nameof(ServiceUnavailable)),
            "{ServiceName} unavailable");

        _serviceAuthorizationFailed = LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(EventIdCodes.ServiceAuthorizationFailed, nameof(ServiceAuthorizationFailed)),
            "Authorization failed");

        _extServiceUnavailable = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(EventIdCodes.ExtServiceUnavailable, nameof(ExternalServiceUnavailable)),
            "Some of underlying services for '{ParentServiceName}' are not available or failed to call");

        _extServiceAuthenticationFailed = LoggerMessage.Define(
            LogLevel.Error,
            new EventId(EventIdCodes.ExtServiceAuthenticationFailed, nameof(ExternalServiceAuthenticationFailed)),
            "Authentication failed");

        _extServiceResponseError = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(EventIdCodes.ExtServiceResponseError, nameof(ExternalServiceResponseError)),
            "Error returned from external service: {Message}");

        _extServiceServerError = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(EventIdCodes.ExtServiceServerError, nameof(ExternalServiceServerError)),
            "External service internal error: {Message}");
    }

    /// <summary>
    /// Nepodařilo se autentizovat do služby.
    /// </summary>
    public static void ExternalServiceAuthenticationFailed(this ILogger logger, Exception ex)
        => _extServiceAuthenticationFailed(logger, ex);

    public static void ServiceAuthorizationFailed(this ILogger logger, Exception ex)
        => _serviceAuthorizationFailed(logger, ex);

    /// <summary>
    /// Doménová služba není dostupná.
    /// </summary>
    /// <param name="serviceName">Název služby</param>
    public static void ServiceUnavailable(this ILogger logger, in string serviceName, Exception ex)
        => _serviceUnavailable(logger, serviceName, ex);

    /// <summary>
    /// Služba třetí strany není dostupná.
    /// </summary>
    /// <param name="parentServiceName">Název doménové služby, ze které je služba třetí strany volaná.</param>
    public static void ExternalServiceUnavailable(this ILogger logger, in string parentServiceName, Exception ex)
        => _extServiceUnavailable(logger, parentServiceName, ex);

    public static void ExternalServiceServerError(this ILogger logger, in string parentServiceName, Exception ex)
        => _extServiceServerError(logger, parentServiceName, ex);

    /// <summary>
    /// Pokud služba třetí strany vrátí nestandardní chybové hlášení - ne 500 nebo 400 a chceme to zalogovat.
    /// </summary>
    /// <remarks>
    /// Jedná se třeba o služby EAS, které nemají žádný error handling.
    /// </remarks>
    public static void ExternalServiceResponseError(this ILogger logger, in string message)
        => _extServiceResponseError(logger, message, null!);
}
