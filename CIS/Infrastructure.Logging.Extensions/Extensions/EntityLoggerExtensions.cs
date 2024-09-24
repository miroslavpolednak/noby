#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

/// <summary>
/// Extension metody pro ILogger pro události týkající se entit.
/// </summary>
public static class EntityLoggerExtensions
{
    private static readonly Action<ILogger, string, object, Exception> _entityAlreadyExist = LoggerMessage.Define<string, object>(
            LogLevel.Warning,
            new EventId(EventIdCodes.EntityAlreadyExist, nameof(EntityAlreadyExist)),
            "{EntityName} #{Id} already exist");

    private static readonly Action<ILogger, string, Exception> _entityAlreadyExistMessage = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(EventIdCodes.EntityAlreadyExist, nameof(EntityAlreadyExist)),
            "{Message}");

    private static readonly Action<ILogger, string, object, Exception> _entityNotFound = LoggerMessage.Define<string, object>(
           LogLevel.Warning,
           new EventId(EventIdCodes.EntityNotFound, nameof(EntityNotFound)),
           "{EntityName} #{Id} not found");

    private static readonly Action<ILogger, string, Exception> _entityNotFoundMessage = LoggerMessage.Define<string>(
           LogLevel.Warning,
           new EventId(EventIdCodes.EntityNotFound, nameof(EntityNotFound)),
           "{Message}");

    private static readonly Action<ILogger, string, long, Exception> _entityCreated = LoggerMessage.Define<string, long>(
            LogLevel.Information,
            new EventId(EventIdCodes.EntityCreated, nameof(EntityCreated)),
            "{EntityName} created with #{Id}");

    private static readonly Action<ILogger, string, Exception> _databaseRollbackInitiated = LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(EventIdCodes.DatabaseRollbackInitiated, nameof(DatabaseRollbackInitiated)),
            "Database rollback: {ExceptionMessage}");

    public static void DatabaseRollbackInitiated(this ILogger logger, Exception ex)
        => _databaseRollbackInitiated(logger, ex.Message, ex);

    /// <summary>
    /// Entita již existuje (např. v databázi).
    /// </summary>
    /// <param name="entityName">Název typu entity</param>
    /// <param name="entityId">ID entity</param>
    public static void EntityAlreadyExist(this ILogger logger, in string entityName, in long entityId, Exception ex)
        => _entityAlreadyExist(logger, entityName, entityId, ex);

    /// <summary>
    /// Entita již existuje (např. v databázi).
    /// </summary>
    public static void EntityAlreadyExist(this ILogger logger, Core.Exceptions.CisAlreadyExistsException ex)
    {
        if (ex.GetId() == null)
            _entityAlreadyExistMessage(logger, ex.Message, ex);
        else
            _entityAlreadyExist(logger, ex.EntityName!, ex.GetId()!, ex);
    }

    /// <summary>
    /// Entita nebyla nalezena (např. ID neexistuje v databázi)
    /// </summary>
    /// <param name="entityName">Název typu entity</param>
    /// <param name="entityId">ID entity</param>
    public static void EntityNotFound(this ILogger logger, in string entityName, in long entityId, Exception ex)
        => _entityNotFound(logger, entityName, entityId, ex);

    /// <summary>
    /// Entita nebyla nalezena (např. ID neexistuje v databázi)
    /// </summary>
    public static void EntityNotFound(this ILogger logger, Core.Exceptions.CisNotFoundException ex)
    {
        if (ex.GetId() == null)
            _entityNotFoundMessage(logger, ex.Message, ex);
        else
            _entityNotFound(logger, ex.EntityName!, ex.GetId()!, ex);
    }

    /// <summary>
    /// Entita byla právě vytvořena.
    /// </summary>
    /// <param name="entityName">Název typu entity</param>
    /// <param name="entityId">ID nové entity</param>
    public static void EntityCreated(this ILogger logger, in string entityName, in long entityId)
        => _entityCreated(logger, entityName, entityId, null!);
}
