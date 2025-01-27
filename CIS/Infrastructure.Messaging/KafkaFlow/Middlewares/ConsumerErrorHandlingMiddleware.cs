﻿using System.Text;
using CIS.Core.Exceptions;
using KafkaFlow;
using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Middlewares;

internal sealed class ConsumerErrorHandlingMiddleware : IMessageMiddleware
{
    private readonly ILogger<ConsumerErrorHandlingMiddleware> _logger;

    public ConsumerErrorHandlingMiddleware(ILogger<ConsumerErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BaseCisException ex)
        {
            _logger.ConsumingMessageFailed(GetMessageId(context), context.ConsumerContext.Topic, ex);
        }
        catch (Exception ex)
        {
            var loggerData = new Dictionary<string, object>
            {
                { nameof(context.ConsumerContext.ConsumerName), context.ConsumerContext.ConsumerName }
            };

            if (context.Message.Value is not null && context.Message.Value is not byte[])
                loggerData.Add(nameof(context.Message), context.Message.Value);

            using (_logger.BeginScope(loggerData))
            {
                _logger.ConsumingMessageFailed(GetMessageId(context), context.ConsumerContext.Topic, ex);
            }
        }

        context.ConsumerContext.Complete();
    }

    private static string GetMessageId(IMessageContext context)
    {
        var messageId = context.Headers.GetString("messaging.id", Encoding.UTF8) ?? Encoding.UTF8.GetString(context.Message.Key as byte[] ?? []);

        return string.IsNullOrWhiteSpace(messageId) ? "N/A" : messageId;
    }
}