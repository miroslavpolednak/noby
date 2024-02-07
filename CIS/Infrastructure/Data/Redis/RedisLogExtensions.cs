namespace CIS.Infrastructure.Data.Redis;

internal static class RedisLogExtensions
{
    private static readonly Action<ILogger, string, Exception> _unknownConsumerException;
    private static readonly Action<ILogger, string, Exception> _unknownMessage;
    private static readonly Action<ILogger, string, Exception> _unknownMessageForAssembly;
    private static readonly Action<ILogger, string, string, string, Exception> _startProcessingMessage;
    private static readonly Action<ILogger, string, string, string, Exception> _endProcessingMessage;
    private static readonly Action<ILogger, string, string, Exception> _messageAddedToChanel;
    private static readonly Action<ILogger, string, string, Exception> _messageAddedToQueue;

    static RedisLogExtensions()
    {
        _unknownConsumerException = LoggerMessage.Define<string>(
          LogLevel.Error,
          new EventId(801, nameof(UnknownConsumerException)),
          "{Description}");

        _unknownMessage = LoggerMessage.Define<string>(
        LogLevel.Information,
        new EventId(802, nameof(UnknownMessage)),
        " Unknown message {Message}, message won't be processed");

        _unknownMessageForAssembly = LoggerMessage.Define<string>(
         LogLevel.Information,
         new EventId(803, nameof(UnknownMessageForAssembly)),
         "Unknown message {TypeName} for consumer assembly, message won't be processed");

        _startProcessingMessage = LoggerMessage.Define<string, string, string>(
       LogLevel.Information,
       new EventId(804, nameof(StartProcessingMessage)),
       "Starting processing message ({MessageName}) from ({Source}) on handler ({HandlerName})");

        _endProcessingMessage = LoggerMessage.Define<string, string, string>(
        LogLevel.Information,
        new EventId(805, nameof(EndProcessingMessage)),
        "Ending processing Message ({MessageName})) from ({Source}) on handler ({HandlerName})");

        _messageAddedToChanel = LoggerMessage.Define<string, string>(
       LogLevel.Information,
       new EventId(806, nameof(MessageAddedToChanel)),
       " Message ({MessageName})) was added to channel ({Channel})");

        _messageAddedToQueue = LoggerMessage.Define<string, string>(
      LogLevel.Information,
      new EventId(807, nameof(MessageAddedToQueue)),
      " Message ({MessageName})) was added to queue ({QueueId})");
       
    }

    public static void UnknownConsumerException(this ILogger logger, Exception ex)
        => _unknownConsumerException(logger, "Unknow subscriber exception:", ex);

    public static void UnknownMessage(this ILogger logger, string message)
        => _unknownMessage(logger, message, null!);

    public static void UnknownMessageForAssembly(this ILogger logger, string typeName)
        => _unknownMessageForAssembly(logger, typeName, null!);

    public static void StartProcessingMessage(this ILogger logger, string source, string messageName, string handlerName)
        => _startProcessingMessage(logger, messageName, source, handlerName, null!);

    public static void EndProcessingMessage(this ILogger logger, string source, string messageName, string handlerName)
       => _endProcessingMessage(logger, messageName, source, handlerName, null!);

    public static void MessageAddedToChanel(this ILogger logger, string channel, string messageName)
        => _messageAddedToChanel(logger, messageName, channel, null!);
    
    public static void MessageAddedToQueue(this ILogger logger, string queueId, string messageName)
       => _messageAddedToQueue(logger, messageName, queueId, null!);
    
}
