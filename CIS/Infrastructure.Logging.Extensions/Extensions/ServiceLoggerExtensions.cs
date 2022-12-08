using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

/// <summary>
/// Extension metody pro ILogger v týkající se handlingu webových služeb třetích stran.
/// </summary>
public static class ServiceLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _serviceUnavailable;
    private static readonly Action<ILogger, string, Exception> _extServiceUnavailable;
    private static readonly Action<ILogger, Exception> _extServiceAuthenticationFailed;

    static ServiceLoggerExtensions()
    {
        _serviceUnavailable = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(EventIdCodes.ServiceUnavailable, nameof(ServiceUnavailable)),
            "{ServiceName} unavailable");

        _extServiceUnavailable = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(EventIdCodes.ExtServiceUnavailable, nameof(ExtServiceUnavailable)),
            "Some of underlying services for '{ParentServiceName}' are not available or failed to call");

        _extServiceAuthenticationFailed = LoggerMessage.Define(
            LogLevel.Warning,
            new EventId(EventIdCodes.ExtServiceAuthenticationFailed, nameof(ExtServiceAuthenticationFailed)),
            "Authentication failed");
    }

    /// <summary>
    /// Nepodařilo se autentizovat do služby.
    /// </summary>
    public static void ExtServiceAuthenticationFailed(this ILogger logger, Exception ex)
        => _extServiceAuthenticationFailed(logger, ex);

    /// <summary>
    /// Doménová služba není dostupná.
    /// </summary>
    /// <param name="serviceName">Název služby</param>
    public static void ServiceUnavailable(this ILogger logger, string serviceName, Exception ex)
        => _serviceUnavailable(logger, serviceName, ex);

    /// <summary>
    /// Služba třetí strany není dostupná.
    /// </summary>
    /// <param name="parentServiceName">Název doménové služby, ze které je služba třetí strany volaná.</param>
    public static void ExtServiceUnavailable(this ILogger logger, string parentServiceName, Exception ex)
        => _extServiceUnavailable(logger, parentServiceName, ex);
}
