using Microsoft.Extensions.Logging;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Contracts.ServiceTypes, string, Exception> _getServiceStarted;
    private static readonly Action<ILogger, string, Contracts.ServiceTypes, string, Exception> _serviceNotFoundInCache;
    private static readonly Action<ILogger, string, Contracts.ServiceTypes, string, Exception> _serviceFoundInDb;
    private static readonly Action<ILogger, string, Contracts.ServiceTypes, string, Exception> _serviceFoundInCache;
    private static readonly Action<ILogger, string, Exception> _discoveryServiceUrlFound;

    static LoggerExtensions()
    {
        _getServiceStarted = LoggerMessage.Define<string, Contracts.ServiceTypes, string>(
            LogLevel.Debug,
            new EventId(151, nameof(GetServiceStarted)),
            "Get {ServiceName}:{ServiceType} from {EnvironmentName}");

        _serviceNotFoundInCache = LoggerMessage.Define<string, Contracts.ServiceTypes, string>(
            LogLevel.Debug,
            new EventId(152, nameof(ServiceNotFoundInCache)),
            "Service {ServiceName}:{ServiceType} from {EnvironmentName} not found in cache");

        _serviceFoundInDb = LoggerMessage.Define<string, Contracts.ServiceTypes, string>(
            LogLevel.Debug,
            new EventId(153, nameof(ServiceFoundInDb)),
            "Service {ServiceName}:{ServiceType} from {EnvironmentName} obtained");

        _serviceFoundInCache = LoggerMessage.Define<string, Contracts.ServiceTypes, string>(
            LogLevel.Debug,
            new EventId(154, nameof(ServiceFoundInCache)),
            "Service {ServiceName}:{ServiceType} from {EnvironmentName} FOUND in cache");

        _discoveryServiceUrlFound = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(155, nameof(DiscoveryServiceUrlFound)),
            "DiscoveryService URL set to '{Url}'");
    }

    public static void GetServiceStarted(this ILogger logger, string serviceName, Contracts.ServiceTypes serviceType, string environmentName)
        => _getServiceStarted(logger, serviceName, serviceType, environmentName, null!);

    public static void ServiceNotFoundInCache(this ILogger logger, string serviceName, Contracts.ServiceTypes serviceType, string environmentName)
        => _serviceNotFoundInCache(logger, serviceName, serviceType, environmentName, null!);

    public static void ServiceFoundInDb(this ILogger logger, string serviceName, Contracts.ServiceTypes serviceType, string environmentName)
        => _serviceFoundInDb(logger, serviceName, serviceType, environmentName, null!);

    public static void ServiceFoundInCache(this ILogger logger, string serviceName, Contracts.ServiceTypes serviceType, string environmentName)
        => _serviceFoundInCache(logger, serviceName, serviceType, environmentName, null!);

    public static void DiscoveryServiceUrlFound(this ILogger logger, string url)
        => _discoveryServiceUrlFound(logger, url, null!);
}
