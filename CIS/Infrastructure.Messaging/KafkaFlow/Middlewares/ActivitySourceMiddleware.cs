using System.Diagnostics;
using KafkaFlow;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Middlewares;

internal sealed class ActivitySourceMiddleware : IMessageMiddleware
{
    private static readonly ActivitySource _activitySource = new(nameof(ActivitySourceMiddleware));

    public ActivitySourceMiddleware()
    {
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

        await next(context);
    }
}