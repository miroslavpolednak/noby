using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;
using Schema = Avro.Schema;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeInfo
{
    public MultipleTypeInfo(Type messageType, Schema schema)
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

    public ISerializerWrapper CreateSerializer(Type messageType, ISchemaRegistryClient schemaRegistryClient,
        AvroSerializerConfig serializerConfig)
    {
        Type t1 = typeof(AvroSerializer<>);
        var constructed1 = t1.MakeGenericType(messageType);
        var inner = Activator.CreateInstance(constructed1, schemaRegistryClient, serializerConfig)!;

        Type t2 = typeof(SerializerWrapper<>);
        var constructed2 = t2.MakeGenericType(messageType);
        return (ISerializerWrapper)Activator.CreateInstance(constructed2, inner)!;
    }
}
