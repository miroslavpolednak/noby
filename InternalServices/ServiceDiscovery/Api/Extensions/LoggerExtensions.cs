namespace CIS.InternalServices.ServiceDiscovery.Api;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _noServicesForEnvironment;
    private static readonly Action<ILogger, int, string, Exception> _foundServices;
    private static readonly Action<ILogger, string, Exception> _servicesFoundInCache;
    private static readonly Action<ILogger, string, Exception> _servicesNotFoundInCache;
    private static readonly Action<ILogger, Exception> _cacheCleared;

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

        _servicesFoundInCache = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(154, nameof(ServicesFoundInCache)),
            "Services for {EnvironmentName} FOUND in cache");

        _servicesNotFoundInCache = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(152, nameof(ServicesNotFoundInCache)),
            "Services for {EnvironmentName} NOT found in cache");

        _cacheCleared = LoggerMessage.Define(
            LogLevel.Information,
            new EventId(158, nameof(CacheCleared)),
            "Cache has been cleared");
    }

    public static void NoServicesForEnvironment(this ILogger logger, string environmentName)
        => _noServicesForEnvironment(logger, environmentName, null!);

    public static void FoundServices(this ILogger logger, int count, string environmentName)
        => _foundServices(logger, count, environmentName, null!);

    public static void ServicesNotFoundInCache(this ILogger logger, string environmentName)
        => _servicesNotFoundInCache(logger, environmentName, null!);

    public static void ServicesFoundInCache(this ILogger logger, string environmentName)
        => _servicesFoundInCache(logger, environmentName, null!);

    public static void CacheCleared(this ILogger logger)
        => _cacheCleared(logger, null!);
}

