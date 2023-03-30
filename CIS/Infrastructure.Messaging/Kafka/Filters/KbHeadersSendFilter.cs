using MassTransit;
using System.Diagnostics;

namespace CIS.Infrastructure.Messaging.Kafka.Filters;

public sealed class KbHeadersSendFilter<T>
    : IFilter<SendContext<T>>
    where T : class
{
    public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        context.Headers.Set("b3", $"{Activity.Current.TraceId}-{Activity.Current.SpanId}-1-{Activity.Current.ParentSpanId}");
        context.Headers.Set("contentType", "avro/binary");
        context.Headers.Set("messaging.id", Guid.NewGuid().ToString("N"));
        context.Headers.Set("messaging.messageType", "SIMPLE");

        context.Headers.Set("messaging.schemaRegistryType", "confluent");
        context.Headers.Set("messaging.timestamp", DateTime.Now.ToUniversalTime().ToString("O"));
        context.Headers.Set("X_HYPHEN_KB_HYPHEN_Orig_HYPHEN_System_HYPHEN_Identity", "NOBY");
        context.Headers.Set("X_HYPHEN_KB_HYPHEN_Caller_HYPHEN_System_HYPHEN_Identity", "NOBY.DS");

        await next.Send(context);
    }

    public void Probe(ProbeContext context) { }
}