using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class ServiceLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _serviceUnavailable;
    private static readonly Action<ILogger, string, Exception> _extServiceUnavailable;

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
    }

    public static void ServiceUnavailable(this ILogger logger, string serviceName, Exception ex)
        => _serviceUnavailable(logger, serviceName, ex);

    public static void ExtServiceUnavailable(this ILogger logger, string parentServiceName, Exception ex)
        => _extServiceUnavailable(logger, parentServiceName, ex);
}
