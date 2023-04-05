using Confluent.Kafka;
using Confluent.SchemaRegistry;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeJsonDeserializer<T> : IAsyncDeserializer<T>
{
    private readonly ISchemaRegistryClient _schemaRegistryClient;
    private readonly MultipleTypeJsonConfig _typeJsonConfig;
    
    public MultipleTypeJsonDeserializer(MultipleTypeJsonConfig typeJsonConfig, ISchemaRegistryClient schemaRegistryClient)
    {
        _typeJsonConfig = typeJsonConfig;
        _schemaRegistryClient = schemaRegistryClient;
    }
    
    public Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
    {
        throw new NotImplementedException();
    }
}