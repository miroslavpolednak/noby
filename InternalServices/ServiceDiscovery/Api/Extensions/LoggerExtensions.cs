namespace CIS.InternalServices.ServiceDiscovery.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _noServicesForEnvironment;
    private static readonly Action<ILogger, int, string, Exception> _foundServices;
    private static readonly Action<ILogger, string, Contracts.ServiceTypes, string, Exception> _serviceFoundInCache;
    private static readonly Action<ILogger, string, Contracts.ServiceTypes, string, Exception> _serviceNotFound;

    static LoggerExtensions()
    {
        _noServicesForEnvironment = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(156, nameof(NoServicesForEnvironment)),
            "No services exists for environment '{EnvironmentName}'");

        _foundServices = LoggerMessage.Define<int, string>(
            LogLevel.Debug,
            new EventId(157, nameof(FoundServices)),
            "Database: found {Count} services in {EnvironmentName}");

        _serviceFoundInCache = LoggerMessage.Define<string, Contracts.ServiceTypes, string>(
            LogLevel.Debug,
            new EventId(154, nameof(ServiceFoundInCache)),
            "Service {ServiceName}:{ServiceType} from {EnvironmentName} FOUND in cache");

        _serviceNotFound = LoggerMessage.Define<string, Contracts.ServiceTypes, string>(
            LogLevel.Warning,
            new EventId(158, nameof(ServiceNotFound)),
            "Service {ServiceName}:{ServiceType} from {EnvironmentName} FOUND in cache");
    }

    public static void NoServicesForEnvironment(this ILogger logger, string environmentName)
        => _noServicesForEnvironment(logger, environmentName, null!);

    public static void FoundServices(this ILogger logger, int count, string environmentName)
        => _foundServices(logger, count, environmentName, null!);

    public static void ServiceFoundInCache(this ILogger logger, string serviceName, Contracts.ServiceTypes serviceType, string environmentName)
        => _serviceFoundInCache(logger, serviceName, serviceType, environmentName, null!);

    public static void ServiceNotFound(this ILogger logger, string serviceName, Contracts.ServiceTypes serviceType, string environmentName)
        => _serviceNotFound(logger, serviceName, serviceType, environmentName, null!);
}
