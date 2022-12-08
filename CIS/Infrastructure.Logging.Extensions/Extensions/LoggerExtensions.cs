using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, object, Exception> _logSerializedObject;
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

        _logSerializedObject = LoggerMessage.Define<string, object>(
            LogLevel.Debug,
            new EventId(EventIdCodes.LogSerializedObject, nameof(LogSerializedObject)),
            "{Name} serialized to: {@Object}");
    }

    /// <summary>
    /// Nalezeno záznamů / entit / objektů.
    /// </summary>
    /// <remarks>
    /// Např. entit v databázi nebo položek v keši.
    /// </remarks>
    /// <param name="count">Počet nalezených záznamů.</param>
    public static void FoundItems(this ILogger logger, int count)
        => _foundItems(logger, count, null!);

    /// <summary>
    /// Nalezeno záznamů / entit / objektů.
    /// </summary>
    /// <remarks>
    /// Např. entit v databázi nebo položek v keši.
    /// </remarks>
    /// <param name="count">Počet nalezených záznamů.</param>
    /// <param name="entityName">Typ nalezené entity</param>
    public static void FoundItems(this ILogger logger, int count, string entityName)
        => _foundItemsWithName(logger, count, entityName, null!);

    /// <summary>
    /// TODO: odstranit? Logovat do log contextu?
    /// </summary>
    public static void LogSerializedObject(this ILogger logger, string name, object objectToLog, LogLevel logLevel = LogLevel.Debug)
        => _logSerializedObject(logger, name, objectToLog, null!);
}
