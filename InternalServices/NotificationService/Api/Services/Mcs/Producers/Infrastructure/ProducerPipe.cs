using Avro.Specific;
using MassTransit;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers.Infrastructure;

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
        context.Headers.Set( "messaging.id", _id );
        context.Headers.Set( "messaging.messageType", "EVENT" );
        context.Headers.Set( "messaging.kafka.payloadTypeId", context.Message.Schema.Fullname );
        context.Headers.Set( "messaging.kafka.replyTopic", _replyTopic );
        context.Headers.Set( "messaging.kafka.replyBrokerUri", _replyBrokerUri );
        context.Headers.Set( "messaging.timestamp", _now.ToUniversalTime().ToString("yyyy-MM-dd hh:mm:ssZ") );

        return Task.CompletedTask;
    }

    public void Probe(ProbeContext context)
    {
    }
}