namespace CIS.InternalServices.ServiceDiscovery.Clients;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Contracts.ServiceTypes, string, Exception> _getServiceStarted;
    private static readonly Action<ILogger, string, Exception> _servicesNotFoundInCache;
    private static readonly Action<ILogger, string, Contracts.ServiceTypes, string, Exception> _serviceFoundInDb;
    private static readonly Action<ILogger, string, Exception> _servicesFoundInCache;
    private static readonly Action<ILogger, string, Exception> _discoveryServiceUrlFound;

    static LoggerExtensions()
    {
        _getServiceStarted = LoggerMessage.Define<string, Contracts.ServiceTypes, string>(
            LogLevel.Debug,
            new EventId(161, nameof(GetServiceStarted)),
            "Get {ServiceName}:{ServiceType} from {EnvironmentName}");

        _servicesNotFoundInCache = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(162, nameof(ServicesNotFoundInCache)),
            "Services for {EnvironmentName} NOT found in cache");

        _serviceFoundInDb = LoggerMessage.Define<string, Contracts.ServiceTypes, string>(
            LogLevel.Debug,
            new EventId(163, nameof(ServiceFoundInDb)),
            "Service {ServiceName}:{ServiceType} from {EnvironmentName} obtained");

        _servicesFoundInCache = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(164, nameof(ServicesFoundInCache)),
            "Services for {EnvironmentName} FOUND in cache");

        _discoveryServiceUrlFound = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(165, nameof(DiscoveryServiceUrlFound)),
            "DiscoveryService URL set to '{Url}'");
    }

    public static void GetServiceStarted(this ILogger logger, string serviceName, Contracts.ServiceTypes serviceType, string environmentName)
        => _getServiceStarted(logger, serviceName, serviceType, environmentName, null!);

    public static void ServicesNotFoundInCache(this ILogger logger, string environmentName)
        => _servicesNotFoundInCache(logger, environmentName, null!);

    public static void ServiceFoundInDb(this ILogger logger, string serviceName, Contracts.ServiceTypes serviceType, string environmentName)
        => _serviceFoundInDb(logger, serviceName, serviceType, environmentName, null!);

    public static void ServicesFoundInCache(this ILogger logger, string environmentName)
        => _servicesFoundInCache(logger, environmentName, null!);

    public static void DiscoveryServiceUrlFound(this ILogger logger, string url)
        => _discoveryServiceUrlFound(logger, url, null!);
}
