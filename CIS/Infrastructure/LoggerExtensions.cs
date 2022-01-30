namespace CIS.Infrastructure.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _requestHandlerStarted;
    private static readonly Action<ILogger, string, long, Exception> _requestHandlerStartedWithId;
    private static readonly Action<ILogger, string, Exception> _requestHandlerFinished;
    private static readonly Action<ILogger, string, long, Exception> _entityAlreadyExist;
    private static readonly Action<ILogger, int, Exception> _foundItems;
    private static readonly Action<ILogger, string, long, Exception> _entityCreated;

    static LoggerExtensions()
    {
        _requestHandlerStarted = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(501, nameof(RequestHandlerStarted)),
            "Request in {HandlerName} started");

        _requestHandlerStartedWithId = LoggerMessage.Define<string, long>(
            LogLevel.Debug,
            new EventId(502, nameof(RequestHandlerStartedWithId)),
            "Request in {HandlerName} started with ID {Id}");

        _requestHandlerFinished = LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(503, nameof(RequestHandlerFinished)),
            "Request in {HandlerName} finished");

        _entityAlreadyExist = LoggerMessage.Define<string, long>(
            LogLevel.Error,
            new EventId(504, nameof(EntityAlreadyExist)),
            "{EntityName} #{Id} already exist in DB");

        _foundItems = LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(505, nameof(FoundItems)),
            "Found {Count} items");

        _entityCreated = LoggerMessage.Define<string, long>(
            LogLevel.Error,
            new EventId(506, nameof(EntityCreated)),
            "{EntityName} created with #{Id}");
    }

    public static void RequestHandlerStarted(this ILogger logger, string handlerName)
        => _requestHandlerStarted(logger, handlerName, null!);

    public static void EntityAlreadyExist(this ILogger logger, string entityName, long entityId, Exception ex)
        => _entityAlreadyExist(logger, entityName, entityId, ex);

    public static void RequestHandlerStartedWithId(this ILogger logger, string handlerName, long id)
        => _requestHandlerStartedWithId(logger, handlerName, id, null!);

    public static void RequestHandlerFinished(this ILogger logger, string handlerName)
    => _requestHandlerFinished(logger, handlerName, null!);

    public static void FoundItems(this ILogger logger, int count)
        => _foundItems(logger, count, null!);

    public static void EntityCreated(this ILogger logger, string entityName, long entityId)
        => _entityCreated(logger, entityName, entityId, null!);
}
