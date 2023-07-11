using CIS.Infrastructure.Messaging.Kafka;
using KB.Speed.Messaging.Kafka.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace CIS.Infrastructure.Messaging;

internal sealed class CisMessagingBuilder
    : ICisMessagingBuilder
{
    public ICisMessagingKafkaBuilder AddKafka(Assembly? assembly = null)
    {
        var configuration = AppBuilder.GetKafkaRiderConfiguration();

        AppBuilder.Services.AddAvroSerializerConfiguration();
        AppBuilder.Services.AddAvroDeserializerConfiguration();
        AppBuilder.Services.AddJsonSerializerConfiguration();
        AppBuilder.Services.AddJsonDeserializerConfiguration();
        AppBuilder.Services.AddApicurioSchemaRegistry();

        if (assembly is null)
        {
            assembly = Assembly.GetEntryAssembly()!;
        }
        return new CisMessagingKafkaBuilder(this, configuration, assembly);
    }

    internal readonly WebApplicationBuilder AppBuilder;

    public CisMessagingBuilder(WebApplicationBuilder builder)
    {
        AppBuilder = builder;
    }
}
