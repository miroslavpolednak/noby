using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Messaging;

internal static class LoggerExtensions
{
    private static readonly Action<ILogger, string, string, Exception> _consumingMessageFailed =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(901, nameof(ConsumingMessageFailed)),
            "Failed to consume message with ID '{MessageId}' from {Topic}"
        );

    private static readonly Action<ILogger, Exception> _schemaRegistryError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(902, nameof(SchemaRegistryError)),
            "Schema registry error"
        );

    private static readonly Action<ILogger, string, string, string, Exception> _consumingKnownMessage =
        LoggerMessage.Define<string, string, string>(
            LogLevel.Information,
            new EventId(903, nameof(ConsumingKnownMessage)),
            "Consuming message type {MessageType} with ID '{MessageId}' from {Topic}"
        );

    // public accessor
    public static void ConsumingMessageFailed(this ILogger logger, in string messageId, in string topic, Exception ex) => 
        _consumingMessageFailed(logger, messageId, topic, ex);

    public static void SchemaRegistryError(this ILogger logger, Exception ex) =>
        _schemaRegistryError(logger, ex);

    public static void ConsumingKnownMessage(this ILogger logger, in string messageType, in string messageId, in string topic) =>
        _consumingKnownMessage(logger, messageType, messageId, topic, null!);
}
