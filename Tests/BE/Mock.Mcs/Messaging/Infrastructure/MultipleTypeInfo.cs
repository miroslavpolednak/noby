﻿using Avro.Specific;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Schema = Avro.Schema;

namespace Mock.Mcs.Messaging.Infrastructure;

public abstract class MultipleTypeInfo
{
    protected MultipleTypeInfo(Type messageType, Schema schema)
    {
        MessageType = messageType ?? throw new ArgumentNullException(nameof(messageType));
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
    }

    public Type MessageType { get; }
    
    public Schema Schema { get; }

    public abstract IReaderWrapper CreateReader(Schema writerSchema);

    public abstract ISerializerWrapper CreateSerializer(ISchemaRegistryClient schemaRegistryClient,
        AvroSerializerConfig serializerConfig);
}

public class MultipleTypeInfo<T> : MultipleTypeInfo
    where T : ISpecificRecord
{
    public MultipleTypeInfo(Type messageType, Schema schema) : base(messageType, schema)
    {
    }

    public override IReaderWrapper CreateReader(Schema writerSchema) => new ReaderWrapper<T>(writerSchema, Schema);

    public override ISerializerWrapper CreateSerializer(ISchemaRegistryClient schemaRegistryClient, AvroSerializerConfig serializerConfig)
    {
        var inner = new AvroSerializer<T>(schemaRegistryClient, serializerConfig);
        return new SerializerWrapper<T>(inner);
    }
}