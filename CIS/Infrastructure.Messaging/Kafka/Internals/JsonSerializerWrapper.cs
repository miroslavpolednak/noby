using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;
using KB.Speed.Messaging.Kafka.SerDes.Json;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

internal sealed class JsonSerializerWrapper<T> : ISerializerWrapper where T : class
{
    private readonly JsonSerializer<T> _inner;

    public JsonSerializerWrapper(JsonSerializer<T> inner)
    {
        _inner = inner;
    }

    public async Task<byte[]> SerializeAsync(object data, SerializationContext context)
    {
        return await _inner.SerializeAsync((T)data, context).ConfigureAwait(false);
    }
}