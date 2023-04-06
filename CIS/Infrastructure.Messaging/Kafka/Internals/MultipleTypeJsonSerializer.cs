using Confluent.Kafka;
using Confluent.SchemaRegistry;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;
using KB.Speed.Messaging.Kafka.SerDes.Json;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeJsonSerializer<T> : IAsyncSerializer<T>
{
    private readonly MultipleTypeJsonConfig _typeJsonConfig;
    private readonly ISchemaRegistryClient _schemaRegistryClient;
    private readonly JsonSerializerConfig _serializerConfig;
    private readonly Dictionary<Type, ISerializerWrapper> _serializers = new();

    public MultipleTypeJsonSerializer(
        MultipleTypeJsonConfig typeJsonConfig,
        ISchemaRegistryClient schemaRegistryClient,
        JsonSerializerConfig serializerConfig)
    {
        _typeJsonConfig = typeJsonConfig;
        _schemaRegistryClient = schemaRegistryClient;
        _serializerConfig = serializerConfig;
        InitialiseSerializers();
    }

    private void InitialiseSerializers()
    {
        foreach (var typeInfo in _typeJsonConfig.Types)
        {
            var serializer = MultipleTypeJsonInfo.CreateJsonSerializer(typeInfo.MessageType, _schemaRegistryClient, _serializerConfig);
            _serializers[typeInfo.MessageType] = serializer;
        }
    }

    public async Task<byte[]> SerializeAsync(T data, SerializationContext context)
    {
        var type = data.GetType();
        if (!_serializers.TryGetValue(type, out var serializer))
        {
            throw new ArgumentException(
                $"Unexpected type {type}. All types to be serialized need to be registered in the {nameof(MultipleTypeJsonConfig)} that is supplied to this instance of {nameof(MultipleTypeJsonSerializer<T>)}",
                nameof(data));
        }

        return await serializer.SerializeAsync(data!, context);
    }
}