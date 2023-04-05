using Avro.Specific;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeAvroSerializer<T> : IAsyncSerializer<T>
{
    private readonly MultipleTypeAvroConfig _typeAvroConfig;
    private readonly ISchemaRegistryClient _schemaRegistryClient;
    private readonly AvroSerializerConfig _serializerConfig;
    private readonly Dictionary<string, ISerializerWrapper> _serializers = new();

    public MultipleTypeAvroSerializer(MultipleTypeAvroConfig typeAvroConfig,
        ISchemaRegistryClient schemaRegistryClient,
        AvroSerializerConfig serializerConfig)
    {
        _typeAvroConfig = typeAvroConfig;
        _schemaRegistryClient = schemaRegistryClient;
        _serializerConfig = serializerConfig;
        InitialiseSerializers();
    }

    private void InitialiseSerializers()
    {
        foreach (var typeInfo in _typeAvroConfig.Types)
        {
            var serializer = MultipleTypeAvroInfo.CreateAvroSerializer(typeInfo.MessageType, _schemaRegistryClient, _serializerConfig);
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
                $"Unexpected type {fullName}. All types to be serialized need to be registered in the {nameof(MultipleTypeAvroConfig)} that is supplied to this instance of {nameof(MultipleTypeAvroSerializer<T>)}",
                nameof(data));
        }

        return await serializer.SerializeAsync(data!, context);
    }
}