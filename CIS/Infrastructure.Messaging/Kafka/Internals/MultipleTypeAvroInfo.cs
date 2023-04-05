using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;
using KB.Speed.Messaging.Kafka.SerDes.Json;
using Schema = Avro.Schema;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeAvroInfo
{
    public MultipleTypeAvroInfo(Type messageType, Schema schema)
    {
        MessageType = messageType ?? throw new ArgumentNullException(nameof(messageType));
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
    }

    public Type MessageType { get; }
    
    public Schema Schema { get; }

    public IReaderWrapper CreateReader(Type messageType, Schema writerSchema)
    {
        Type t = typeof(ReaderWrapper<>);
        var constructed = t.MakeGenericType(messageType);
        return (IReaderWrapper)Activator.CreateInstance(constructed, writerSchema, Schema)!;
    }

    public static ISerializerWrapper CreateAvroSerializer(Type messageType, ISchemaRegistryClient schemaRegistryClient,
        AvroSerializerConfig serializerConfig)
    {
        Type t1 = typeof(AvroSerializer<>);
        var constructed1 = t1.MakeGenericType(messageType);
        var inner = Activator.CreateInstance(constructed1, schemaRegistryClient, serializerConfig)!;

        Type t2 = typeof(AvroSerializerWrapper<>);
        var constructed2 = t2.MakeGenericType(messageType);
        return (ISerializerWrapper)Activator.CreateInstance(constructed2, inner)!;
    }
    
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
}
