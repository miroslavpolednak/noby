﻿using Avro.Specific;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace Mock.Mcs.Messaging.Infrastructure;

public class MultipleTypeSerializer<T> : IAsyncSerializer<T>
{
    private readonly MultipleTypeConfig _typeConfig;
    private readonly ISchemaRegistryClient _schemaRegistryClient;
    private readonly AvroSerializerConfig _serializerConfig;
    private readonly Dictionary<string, ISerializerWrapper> _serializers = new();

    public MultipleTypeSerializer(MultipleTypeConfig typeConfig,
        ISchemaRegistryClient schemaRegistryClient,
        AvroSerializerConfig serializerConfig = null)
    {
        _typeConfig = typeConfig;
        _schemaRegistryClient = schemaRegistryClient;
        _serializerConfig = serializerConfig;
        InitialiseSerializers();
    }

    private void InitialiseSerializers()
    {
        foreach (var typeInfo in _typeConfig.Types)
        {
            var serializer = typeInfo.CreateSerializer(_schemaRegistryClient, _serializerConfig);
            _serializers[typeInfo.Schema.Fullname] = serializer;
        }
    }

    public async Task<byte[]> SerializeAsync(T data, SerializationContext context)
    {
        var record = data as ISpecificRecord;
        if (record == null)
        {
            throw new ArgumentException(
                $"Object being serialized is not an instance of {nameof(ISpecificRecord)}. This serializer only serializes types generated using the avrogen.exe tool.",
                nameof(data));
        }

        var fullName = record.Schema.Fullname;
        if (!_serializers.TryGetValue(fullName, out var serializer))
        {
            throw new ArgumentException(
                $"Unexpected type {fullName}. All types to be serialized need to be registered in the {nameof(MultipleTypeConfig)} that is supplied to this instance of {nameof(MultipleTypeSerializer<T>)}",
                nameof(data));
        }

        return await serializer.SerializeAsync(data, context);
    }
}