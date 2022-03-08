using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class CacheLoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _itemFoundInCache;
    private static readonly Action<ILogger, string, Exception> _tryAddItemToCache;
    
    static CacheLoggerExtensions()
    {
        _itemFoundInCache = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.ItemFoundInCache, nameof(ItemFoundInCache)),
            "Item with key '{Key}' found in cache");

        _tryAddItemToCache = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.TryAddItemToCache, nameof(TryAddItemToCache)),
            "Try to add key '{Key}' to cache");
    }

    public static void ItemFoundInCache(this ILogger logger, string key)
        => _itemFoundInCache(logger, key, null!);
    
    public static void TryAddItemToCache(this ILogger logger, string key)
        => _tryAddItemToCache(logger, key, null!);
}