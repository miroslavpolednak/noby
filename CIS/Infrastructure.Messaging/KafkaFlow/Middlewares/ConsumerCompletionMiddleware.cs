using KafkaFlow;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Middlewares;

public class ConsumerCompletionMiddleware : IMessageMiddleware
{
    public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
    {
        await next(context);

        context.ConsumerContext.Complete();
    }
}