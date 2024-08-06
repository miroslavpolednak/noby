using KafkaFlow;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using CIS.Infrastructure.Messaging.Configuration;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Middlewares;

internal sealed class LoggingKnownMessagesMiddleware(
    KafkaFlowConfiguration _configuration, 
    ILogger<LoggingKnownMessagesMiddleware> _logger) 
    : IMessageMiddleware
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreReadOnlyProperties = true, IgnoreReadOnlyFields = true };

    public Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        if (context.Message.Value is null or byte[])
            return next(context);

        var messagingId = context.Headers.GetString("messaging.id", Encoding.UTF8);

        var loggerData = new Dictionary<string, object>
        {
            { "ConsumerName", context.ConsumerContext.ConsumerName },
            { "MessageType", context.Message.Value.GetType().FullName! },
            { "KafkaOffset", context.ConsumerContext.Offset }
        };

        if (_configuration.LogConsumingMessagePayload)
        {
            var messageData = JsonSerializer.Serialize(context.Message.Value, _jsonSerializerOptions);

            loggerData.Add("Payload", messageData);
        }

        using (_logger.BeginScope(loggerData))
        {
            _logger.ConsumingKnownMessage(context.Message.Value.GetType().FullName!, messagingId, context.ConsumerContext.Topic);
        }

        return next(context);
    }
}