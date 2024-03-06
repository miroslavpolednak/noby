using System.Diagnostics;
using System.Text;
using KafkaFlow;
using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Middlewares;

internal sealed class ConsumerLoggingMiddleware : IMessageMiddleware
{
    private readonly ILogger<ConsumerLoggingMiddleware> _logger;
    private static readonly ActivitySource _activitySource = new(nameof(IMessageMiddleware));

    private static readonly Action<ILogger, string, Exception> _messageStartingProcessing = LoggerMessage.Define<string>(
        LogLevel.Information,
        new EventId(901, nameof(ConsumerLoggingMiddleware)),
        "Starting to process a message with ID '{MessageId}'");

    private static readonly Action<ILogger, string, Exception> _messageProcessingFinished = LoggerMessage.Define<string>(
        LogLevel.Information,
        new EventId(902, nameof(ConsumerLoggingMiddleware)),
        "Completed message processing with ID '{MessageId}'");

    public ConsumerLoggingMiddleware(ILogger<ConsumerLoggingMiddleware> logger)
    {
        _logger = logger;

        ActivitySource.AddActivityListener(new ActivityListener
        {
            ShouldListenTo = _ => true,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
        });
    }

    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        using var activity = _activitySource.StartActivity(ActivityKind.Consumer);

        var messageId = GetMessageId(context);

        _messageStartingProcessing(_logger, messageId, null!);

        await next(context);

        _messageProcessingFinished(_logger, messageId, null!);
    }

    private static string GetMessageId(IMessageContext context)
    {
        var messageId = context.Headers.GetString("messaging.id", Encoding.UTF8) ?? context.Message.Key?.ToString();

        return string.IsNullOrWhiteSpace(messageId) ? "N/A" : messageId;
    }
}