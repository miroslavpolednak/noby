using CIS.Infrastructure.Messaging.Kafka;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace CIS.Infrastructure.Messaging;

internal sealed class CisMessagingBuilder
    : ICisMessagingBuilder
{
    public ICisMessagingKafkaBuilder AddKafka()
    {
        var configuration = AppBuilder.GetKafkaRiderConfiguration();

        AppBuilder.Services.AddAvroSerializerConfiguration();
        AppBuilder.Services.AddAvroDeserializerConfiguration();
        AppBuilder.Services.AddJsonSerializerConfiguration();
        AppBuilder.Services.AddJsonDeserializerConfiguration();
        AppBuilder.Services.AddApicurioSchemaRegistry();

        return new CisMessagingKafkaBuilder(this, configuration);
    }

    internal readonly WebApplicationBuilder AppBuilder;

    public CisMessagingBuilder(WebApplicationBuilder builder)
    {
        AppBuilder = builder;
    }
}
