using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Caching;

internal static class LoggerExtensions
{
    public enum CacheTypes
    {
        Local,
        Distributed
    };

    private static readonly Action<ILogger, CacheTypes, string, string, string, Exception> _usingCacheInsteadOfRpc =
        LoggerMessage.Define<CacheTypes, string, string, string>(
            LogLevel.Debug,
            new EventId(890, nameof(UsingCacheInsteadOfRpc)),
            "Using cached value from {CacheType} for {MethodName} in {ServiceName} with key {Key}"
        );

    // public accessor
    public static void UsingCacheInsteadOfRpc(this ILogger logger, in CacheTypes cacheType, in string key, in string methodName, in string serviceName) =>
        _usingCacheInsteadOfRpc(logger, cacheType, methodName, serviceName, key, null!);

}
