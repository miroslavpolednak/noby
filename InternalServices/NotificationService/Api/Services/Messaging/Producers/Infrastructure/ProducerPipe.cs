using Avro.Specific;
using MassTransit;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;

public class ProducerPipe<TRecord> : IPipe<KafkaSendContext<TRecord>> where TRecord : class, ISpecificRecord
{
    private readonly Headers _headers;

    public ProducerPipe(Headers headers)
    {
        _headers = headers;
    }

    public Task Send(KafkaSendContext<TRecord> context)
    {
        var caller = _headers.Caller;
        var origin = string.IsNullOrEmpty(_headers.Origin) ? caller : _headers.Origin;
        
        // note 1:
        // speed headers and headers for MCS are not same
        // eg. messaging.schemaRegistryType vs. messaging.kafka.schemaRegistryType 
        // context.Headers.Set( "messaging.kafka.schemaRegistryType", "confluent" );
        
        // note 2:
        // some headers are not supported from MCS yet
        // eg. payloadTypeId, replyTopic, replyBrokerUri
        // context.Headers.Set( "messaging.kafka.payloadTypeId", context.Message.Schema.Fullname );
        // context.Headers.Set( "messaging.kafka.replyTopic", _headers.ReplyTopic );
        // context.Headers.Set( "messaging.kafka.replyBrokerUri", _headers.ReplyBrokerUri );
        
        context.Headers.Set( "b3", _headers.B3 ?? "" );
        context.Headers.Set( "contentType", "avro/binary" );
        context.Headers.Set( "messaging.id", _headers.Id );
        context.Headers.Set( "messaging.messageType", "SIMPLE" );

        context.Headers.Set( "messaging.schemaRegistryType", "confluent" );
        context.Headers.Set( "messaging.timestamp", _headers.Timestamp.ToUniversalTime().ToString("O") );
        context.Headers.Set( "X_HYPHEN_KB_HYPHEN_Orig_HYPHEN_System_HYPHEN_Identity", origin );
        context.Headers.Set( "X_HYPHEN_KB_HYPHEN_Caller_HYPHEN_System_HYPHEN_Identity", caller );
        
        return Task.CompletedTask;
    }

    public void Probe(ProbeContext context)
    {
    }
}