using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Logging;

public static class EntityLoggerExtensions
{
    private static readonly Action<ILogger, string, long, Exception> _entityAlreadyExist;
    private static readonly Action<ILogger, string, long, Exception> _entityNotFound;
    private static readonly Action<ILogger, string, long, Exception> _entityCreated;

    static EntityLoggerExtensions()
    {
        _entityAlreadyExist = LoggerMessage.Define<string, long>(
            LogLevel.Error,
            new EventId(EventIdCodes.EntityAlreadyExist, nameof(EntityAlreadyExist)),
            "{EntityName} #{Id} already exist in DB");

        _entityNotFound = LoggerMessage.Define<string, long>(
           LogLevel.Error,
           new EventId(EventIdCodes.EntityNotFound, nameof(EntityNotFound)),
           "{EntityName} #{Id} not found in DB");

        _entityCreated = LoggerMessage.Define<string, long>(
            LogLevel.Information,
            new EventId(EventIdCodes.EntityCreated, nameof(EntityCreated)),
            "{EntityName} created with #{Id}");
    }

    public static void EntityAlreadyExist(this ILogger logger, string entityName, long entityId, Exception ex)
        => _entityAlreadyExist(logger, entityName, entityId, ex);

    public static void EntityAlreadyExist(this ILogger logger, Core.Exceptions.CisAlreadyExistsException ex)
        => _entityAlreadyExist(logger, ex.EntityName!, ex.GetId(), ex);

    public static void EntityNotFound(this ILogger logger, string entityName, long entityId, Exception ex)
        => _entityNotFound(logger, entityName, entityId, ex);

    public static void EntityNotFound(this ILogger logger, Core.Exceptions.CisNotFoundException ex)
        => _entityNotFound(logger, ex.EntityName!, ex.GetId(), ex);

    public static void EntityCreated(this ILogger logger, string entityName, long entityId)
        => _entityCreated(logger, entityName, entityId, null!);
}
