using KafkaFlow.Configuration;
using KafkaFlow.Middlewares.Serializer;
using KafkaFlow.Serializer;

namespace CIS.Infrastructure.Messaging.KafkaFlow.JsonSchema;

internal static class JsonSchemaBuilderExtensions
{
    public static IConsumerMiddlewareConfigurationBuilder AddSchemaRegistryJsonDeserializer(this IConsumerMiddlewareConfigurationBuilder middlewares)
    {
        return middlewares.Add(
            _ => new DeserializerConsumerMiddleware(
                new JsonCoreDeserializer(),
                new JsonSchemaTypeResolver()));
    }
}