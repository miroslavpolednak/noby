using Confluent.SchemaRegistry;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;
using KB.Speed.Messaging.Kafka.SerDes.Json;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeJsonInfo
{
    public MultipleTypeJsonInfo(Type messageType)
    {
        MessageType = messageType ?? throw new ArgumentNullException(nameof(messageType));
    }

    public Type MessageType { get; }

    public static ISerializerWrapper CreateJsonSerializer(Type messageType, ISchemaRegistryClient schemaRegistryClient,
        JsonSerializerConfig serializerConfig)
    {
        Type t1 = typeof(JsonSerializer<>);
        var constructed1 = t1.MakeGenericType(messageType);
        var inner = Activator.CreateInstance(constructed1, schemaRegistryClient, serializerConfig)!;

        Type t2 = typeof(JsonSerializerWrapper<>);
        var constructed2 = t2.MakeGenericType(messageType);
        return (ISerializerWrapper)Activator.CreateInstance(constructed2, inner)!;
    }
    
    public static IDeserializerWrapper CreateJsonDeserializer(Type messageType, ISchemaRegistryClient schemaRegistryClient)
    {
        Type t1 = typeof(JsonDeserializer<>);
        var constructed1 = t1.MakeGenericType(messageType);
        var inner = Activator.CreateInstance(constructed1, schemaRegistryClient)!;

        Type t2 = typeof(JsonDeserializerWrapper<>);
        var constructed2 = t2.MakeGenericType(messageType);
        return (IDeserializerWrapper)Activator.CreateInstance(constructed2, inner)!;
    }
}
