using KafkaFlow;
using Microsoft.Extensions.Logging;
using System.Text;
using CIS.Infrastructure.Messaging.Configuration;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Middlewares;

internal class LoggingKnownMessagesMiddleware : IMessageMiddleware
{
    private readonly KafkaFlowConfiguration _configuration;
    private readonly ILogger<LoggingKnownMessagesMiddleware> _logger;

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
            { "MessageId", messagingId },
            { "Topic", context.ConsumerContext.Topic },
            { "ConsumerName", context.ConsumerContext.ConsumerName },
        };

        if (_configuration.LogConsumingMessagePayload)
            loggerData.Add("Message", context.Message.Value);

        using (_logger.BeginScope(loggerData))
        {
            _logger.ConsumingKnownMessage(messagingId, context.ConsumerContext.Topic);
        }

        return next(context);
    }
}