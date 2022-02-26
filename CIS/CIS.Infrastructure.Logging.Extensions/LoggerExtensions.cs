using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, int, Exception> _foundItems;
    private static readonly Action<ILogger, int, string, Exception> _foundItemsWithName;

    static LoggerExtensions()
    {
        _foundItems = LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(EventIdCodes.FoundItems, nameof(FoundItems)),
            "Found {Count} items");

        _foundItemsWithName = LoggerMessage.Define<int, string>(
            LogLevel.Debug,
            new EventId(EventIdCodes.FoundItems, nameof(FoundItems)),
            "Found {Count} items of type {EntityName}");
    }

    public static void FoundItems(this ILogger logger, int count)
        => _foundItems(logger, count, null!);
    
    public static void FoundItems(this ILogger logger, int count, string entityName)
        => _foundItemsWithName(logger, count, entityName, null!);
}
