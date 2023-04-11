using System.Text;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using KB.Speed.Messaging.Kafka.SerDes.Json;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeJsonDeserializer<T> : IAsyncDeserializer<T>
{
    private readonly MultipleTypeJsonConfig _typeJsonConfig;
    private readonly ISchemaRegistryClient _schemaRegistryClient;
    private readonly JsonDeserializerConfig _deserializerConfig;
    private readonly Dictionary<string, IDeserializerWrapper> _deserializers = new();
    
    public MultipleTypeJsonDeserializer(
        MultipleTypeJsonConfig typeJsonConfig,
        ISchemaRegistryClient schemaRegistryClient,
        JsonDeserializerConfig deserializerConfig)
    {
        _typeJsonConfig = typeJsonConfig;
        _schemaRegistryClient = schemaRegistryClient;
        _deserializerConfig = deserializerConfig;
        InitialiseSerializers();
    }
    
    private void InitialiseSerializers()
    {
        foreach (var typeInfo in _typeJsonConfig.Types)
        {
            var deserializer = MultipleTypeJsonInfo.CreateJsonDeserializer(typeInfo.MessageType, _schemaRegistryClient, _deserializerConfig);
            _deserializers[typeInfo.PayloadId] = deserializer;
        }
    }
    
    public async Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
    {
        var payloadId = GetPayloadId(context);
        if (!_deserializers.TryGetValue(payloadId, out var serializer))
        {
            throw new ArgumentException(
                $"All types to be deserialized need to be registered in the {nameof(MultipleTypeJsonConfig)} that is supplied to this instance of {nameof(MultipleTypeJsonDeserializer<T>)}",
                nameof(data));
        }

        return (T)await serializer.DeserializeAsync(data.ToArray(), context);
    }

    private string GetPayloadId(SerializationContext context)
    {
        var header = context.Headers.Single(h => h.Key == _deserializerConfig.SchemaTypeHeaderKey);
        return Encoding.Default.GetString(header.GetValueBytes());
    }
}