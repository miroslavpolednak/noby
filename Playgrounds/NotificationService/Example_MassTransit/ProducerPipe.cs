using System.Net.Mime;
using Avro.Specific;
using MassTransit;

namespace Example_MassTransit;

public class ProducerPipe<TRecord> : IPipe<KafkaSendContext<TRecord>> where TRecord : class, ISpecificRecord
{
    private readonly string _id;
    private readonly string _replyTopic;
    private readonly string _replyBrokerUri;
    private readonly DateTime _now;

    public ProducerPipe(string id, string replyTopic, string replyBrokerUri, DateTime now)
    {
        _id = id;
        _replyTopic = replyTopic;
        _replyBrokerUri = replyBrokerUri;
        _now = now;
    }

    public Task Send(KafkaSendContext<TRecord> context)
    {
        context.Headers.Set( "b3", "{traceId}-{spanId}-{sampled}-{parentSpanId}" );
        context.Headers.Set( "contentType", "avro/binary" );
        
        context.Headers.Set( "messaging.id", _id );
        context.Headers.Set( "messaging.messageType", "SIMPLE" );
        context.Headers.Set( "messaging.kafka.payloadTypeId", context.Message.Schema.Fullname );
        context.Headers.Set( "messaging.kafka.replyTopic", _replyTopic );
        context.Headers.Set( "messaging.kafka.replyBrokerUri", _replyBrokerUri );
        context.Headers.Set( "messaging.kafka.schemaRegistryType", "confluent" );
        context.Headers.Set( "messaging.timestamp", _now.ToUniversalTime().ToString("O") );
        
        // context.Headers.Set( "X_HYPHEN_B3_HYPHEN_TraceId", "" );
        // context.Headers.Set( "X_HYPHEN_B3_HYPHEN_SpanId", "" );
        // context.Headers.Set( "X_HYPHEN_B3_HYPHEN_Sampled", "" );
        // context.Headers.Set( "X_HYPHEN_B3_HYPHEN_ParentSpanId", "" );
        
        context.Headers.Set( "X_HYPHEN_KB_HYPHEN_Orig_HYPHEN_System_HYPHEN_Identity", "{\"app\":\"NOBY\",\"appComp\":\"NOBY.DS.NotificationService\"}" );
        context.Headers.Set( "X_HYPHEN_KB_HYPHEN_Caller_HYPHEN_System_HYPHEN_Identity", "{\"app\":\"NOBY\",\"appComp\":\"NOBY.DS.NotificationService\"}" );
        
        context.Headers.Set("MessageId", "");
        context.Headers.Set("ConversationId", "");
        context.Headers.Set("DestinationAddress", "");
        context.Headers.Set("SourceAddress", "");

        return Task.CompletedTask;
    }

    public void Probe(ProbeContext context)
    {
    }
}