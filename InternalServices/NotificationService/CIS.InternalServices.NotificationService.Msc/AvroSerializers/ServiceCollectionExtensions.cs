using Avro.Specific;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.NotificationService.Msc.AvroSerializers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAvroSerializers(this IServiceCollection services)
    {
        var avroSchemaTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(type => typeof(ISpecificRecord).IsAssignableFrom(type))
            .ToList();
        
        var serializerInterfaceType = typeof(IAsyncSerializer<>);
        var serializerImplementationType = typeof(AsyncAvroSerializer<>);
        var deserializerInterfaceType = typeof(IAsyncDeserializer<>);
        var deserializerImplementationType = typeof(AsyncAvroDeserializer<>);

        foreach (var avroSchemaType in avroSchemaTypes)
        {
            services
                .AddTransient(
                    serializerInterfaceType.MakeGenericType(avroSchemaType),
                    serializerImplementationType.MakeGenericType(avroSchemaType))
                .AddTransient(
                    deserializerInterfaceType.MakeGenericType(avroSchemaType),
                    deserializerImplementationType.MakeGenericType(avroSchemaType));
        }

        return services;
    }
}