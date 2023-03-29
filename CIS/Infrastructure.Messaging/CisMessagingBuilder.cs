using KB.Speed.Messaging.Kafka.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace CIS.Infrastructure.Messaging;

internal sealed class CisMessagingBuilder
    : ICisMessagingBuilder
{
    private readonly WebApplicationBuilder _builder;

    public CisMessagingBuilder(WebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public Kafka.ICisMessagingKafkaBuilder AddKafka()
    {
        var kafkaBuilder = new Kafka.CisMessagingKafkaBuilder(_builder);

        _builder.Services.AddAvroSerializerConfiguration();
        _builder.Services.AddAvroDeserializerConfiguration();
        _builder.Services.AddApicurioSchemaRegistry();

        return kafkaBuilder;
    }
}
