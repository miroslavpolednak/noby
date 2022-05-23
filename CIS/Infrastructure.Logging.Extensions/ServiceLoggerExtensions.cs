using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class ServiceLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _serviceUnavailable;
    private static readonly Action<ILogger, string, Exception> _extServiceUnavailable;
    private static readonly Action<ILogger, string, int, double, Exception> _extServiceRetryCall;
    private static readonly Action<ILogger, string, string, object, Exception> _extServiceRequest;
    private static readonly Action<ILogger, string, string, object, Exception> _extServiceResponse;

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

        _extServiceRetryCall = LoggerMessage.Define<string, int, double>(
            LogLevel.Warning,
            new EventId(EventIdCodes.ExtServiceRetryCall, nameof(ExtServiceRetryCall)),
            "{Service} delaying for {Delay}ms, then making retry {Retry}");

        _extServiceRequest = LoggerMessage.Define<string, string, object>(
            LogLevel.Debug,
            new EventId(EventIdCodes.ExtServiceRequest, nameof(ExtServiceRequest)),
            "{Service} requests {Url} with payload {@Payload}");

        _extServiceResponse = LoggerMessage.Define<string, string, object>(
            LogLevel.Debug,
            new EventId(EventIdCodes.ExtServiceRequest, nameof(ExtServiceResponse)),
            "{Service} from {Url} responded with {@Response}");
    }

    public static void ServiceUnavailable(this ILogger logger, string serviceName, Exception ex)
        => _serviceUnavailable(logger, serviceName, ex);

    public static void ExtServiceUnavailable(this ILogger logger, string parentServiceName, Exception ex)
        => _extServiceUnavailable(logger, parentServiceName, ex);

    public static void ExtServiceRetryCall(this ILogger logger, string serviceName, int retryAttempt, double delay)
        => _extServiceRetryCall(logger, serviceName, retryAttempt, delay, null!);

    public static void ExtServiceRequest(this ILogger logger, string serviceName, string url, object request)
        => _extServiceRequest(logger, serviceName, url, request, null!);

    public static void ExtServiceResponse(this ILogger logger, string serviceName, string url, object request)
        => _extServiceResponse(logger, serviceName, url, request, null!);
}
