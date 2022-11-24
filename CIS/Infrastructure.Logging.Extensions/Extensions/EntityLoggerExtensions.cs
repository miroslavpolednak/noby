using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

/// <summary>
/// Extension metody pro ILogger pro události týkající se entit.
/// </summary>
public static class EntityLoggerExtensions
{
    private static readonly Action<ILogger, string, object, Exception> _entityAlreadyExist;
    private static readonly Action<ILogger, string, Exception> _entityAlreadyExistMessage;
    private static readonly Action<ILogger, string, object, Exception> _entityNotFound;
    private static readonly Action<ILogger, string, Exception> _entityNotFoundMessage;
    private static readonly Action<ILogger, string, long, Exception> _entityCreated;

    static EntityLoggerExtensions()
    {
        _entityAlreadyExist = LoggerMessage.Define<string, object>(
            LogLevel.Error,
            new EventId(EventIdCodes.EntityAlreadyExist, nameof(EntityAlreadyExist)),
            "{EntityName} #{Id} already exist");

        _entityAlreadyExistMessage = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(EventIdCodes.EntityAlreadyExist, nameof(EntityAlreadyExist)),
            "{Message}");

        _entityNotFound = LoggerMessage.Define<string, object>(
           LogLevel.Error,
           new EventId(EventIdCodes.EntityNotFound, nameof(EntityNotFound)),
           "{EntityName} #{Id} not found");

        _entityNotFoundMessage = LoggerMessage.Define<string>(
           LogLevel.Error,
           new EventId(EventIdCodes.EntityNotFound, nameof(EntityNotFound)),
           "{Message}");

        _entityCreated = LoggerMessage.Define<string, long>(
            LogLevel.Information,
            new EventId(EventIdCodes.EntityCreated, nameof(EntityCreated)),
            "{EntityName} created with #{Id}");
    }

    /// <summary>
    /// Entita již existuje (např. v databázi).
    /// </summary>
    /// <param name="entityName">Název typu entity</param>
    /// <param name="entityId">ID entity</param>
    public static void EntityAlreadyExist(this ILogger logger, string entityName, long entityId, Exception ex)
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
    public static void EntityNotFound(this ILogger logger, string entityName, long entityId, Exception ex)
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
    public static void EntityCreated(this ILogger logger, string entityName, long entityId)
        => _entityCreated(logger, entityName, entityId, null!);
}
