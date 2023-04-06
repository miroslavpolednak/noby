using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;
using Confluent.Kafka;
using Confluent.SchemaRegistry;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeJsonDeserializer<T> : IAsyncDeserializer<T>
{
    private readonly MultipleTypeJsonConfig _typeJsonConfig;
    private readonly ISchemaRegistryClient _schemaRegistryClient;
    private readonly Dictionary<Type, IDeserializerWrapper> _deserializers = new();
    
    public MultipleTypeJsonDeserializer(
        MultipleTypeJsonConfig typeJsonConfig,
        ISchemaRegistryClient schemaRegistryClient)
    {
        _typeJsonConfig = typeJsonConfig;
        _schemaRegistryClient = schemaRegistryClient;
        InitialiseSerializers();
    }
    
    private void InitialiseSerializers()
    {
        foreach (var typeInfo in _typeJsonConfig.Types)
        {
            var deserializer = MultipleTypeJsonInfo.CreateJsonDeserializer(typeInfo.MessageType, _schemaRegistryClient);
            _deserializers[typeInfo.MessageType] = deserializer;
        }
    }
    
    public async Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
    {
        var type = typeof(T);
        if (!_deserializers.TryGetValue(type, out var serializer))
        {
            throw new ArgumentException(
                $"Unexpected type {type}. All types to be deserialized need to be registered in the {nameof(MultipleTypeJsonConfig)} that is supplied to this instance of {nameof(MultipleTypeJsonDeserializer<T>)}",
                nameof(data));
        }

        return (T)await serializer.DeserializeAsync(data.ToArray(), context);
    }
}