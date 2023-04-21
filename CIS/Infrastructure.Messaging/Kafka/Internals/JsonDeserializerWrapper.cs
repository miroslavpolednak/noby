using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;
using KB.Speed.Messaging.Kafka.SerDes.Json;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

internal sealed class JsonDeserializerWrapper<T> : IDeserializerWrapper where T : class
{
    private readonly JsonDeserializer<T> _inner;

    public JsonDeserializerWrapper(JsonDeserializer<T> inner)
    {
        _inner = inner;
    }

    public async Task<object> DeserializeAsync(byte[] data, SerializationContext context)
    {
        return await _inner.DeserializeAsync(data, false, context).ConfigureAwait(false);
    }
}