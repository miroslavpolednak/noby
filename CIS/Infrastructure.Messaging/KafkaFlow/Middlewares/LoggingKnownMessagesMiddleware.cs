using KafkaFlow;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using CIS.Infrastructure.Messaging.Configuration;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Middlewares;

internal class LoggingKnownMessagesMiddleware : IMessageMiddleware
{
    private readonly KafkaFlowConfiguration _configuration;
    private readonly ILogger<LoggingKnownMessagesMiddleware> _logger;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { IgnoreReadOnlyProperties = true, IgnoreReadOnlyFields = true };

    public LoggingKnownMessagesMiddleware(KafkaFlowConfiguration configuration, ILogger<LoggingKnownMessagesMiddleware> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        if (context.Message.Value is null or byte[])
            return next(context);

        var messagingId = context.Headers.GetString("messaging.id", Encoding.UTF8);

        var loggerData = new Dictionary<string, object>
        {
            { "ConsumerName", context.ConsumerContext.ConsumerName },
        };

        if (_configuration.LogConsumingMessagePayload)
        {
            var messageData = JsonSerializer.Serialize(context.Message.Value, _jsonSerializerOptions);

            loggerData.Add("Message", messageData);
        }

        using (_logger.BeginScope(loggerData))
        {
            _logger.ConsumingKnownMessage(messagingId, context.ConsumerContext.Topic);
        }

        return next(context);
    }
}