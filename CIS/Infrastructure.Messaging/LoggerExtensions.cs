using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Messaging;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception> _consumingMessageFailed;
    private static readonly Action<ILogger, Exception> _schemaRegistryError;
    private static readonly Action<ILogger, string, string, Exception> _consumingKnownMessage;

    static LoggerExtensions()
    {
        _consumingMessageFailed = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(901, nameof(ConsumingMessageFailed)),
            "Failed to consume message with ID '{MessageId}'"
        );

        _schemaRegistryError = LoggerMessage.Define(
            LogLevel.Error,
            new EventId(902, nameof(SchemaRegistryError)),
            "Schema registry error"
        );

        _consumingKnownMessage = LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(903, nameof(ConsumingKnownMessage)),
            "Consuming message with ID '{MessageId}' from {TopicId}"
        );
    }

    public static void ConsumingMessageFailed(this ILogger logger, string messageId, Exception ex) => 
        _consumingMessageFailed(logger, messageId, ex);

    public static void SchemaRegistryError(this ILogger logger, Exception ex) =>
        _schemaRegistryError(logger, ex);

    public static void ConsumingKnownMessage(this ILogger logger, string messageId, string topicId) =>
        _consumingKnownMessage(logger, messageId, topicId, null!);
}
