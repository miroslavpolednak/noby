using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, int, Exception> _foundItems;

    static LoggerExtensions()
    {
        _foundItems = LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(EventIdCodes.FoundItems, nameof(FoundItems)),
            "Found {Count} items");

    }

    public static void FoundItems(this ILogger logger, int count)
        => _foundItems(logger, count, null!);
}
