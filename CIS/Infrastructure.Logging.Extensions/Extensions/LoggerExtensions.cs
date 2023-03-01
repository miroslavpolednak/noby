using CIS.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, int, Exception> _foundItems;
    private static readonly Action<ILogger, int, string, Exception> _foundItemsWithName;
    private static readonly Action<ILogger, string, Exception> _logValidationResults;

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

        _logValidationResults = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(EventIdCodes.LogValidationResults, nameof(LogValidationResults)),
            "Validation errors: {Message}");
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
    /// Logování chyb zejména z FluentValidation.
    /// </summary>
    /// <remarks>
    /// Do logu uloží seznam chyb (Errors kolekci) do kontextu pod klíčem "Errors".
    /// </remarks>
    public static void LogValidationResults(this ILogger logger, CisValidationException ex)
    {
        using (logger.BeginScope(new Dictionary<string, object>
        {
            { "Errors", ex.Errors.ToDictionary(k => k.ExceptionCode, v => v.Message) }
        }))
        {
            _logValidationResults(logger, ex.Message, ex);
        }
    }
}
