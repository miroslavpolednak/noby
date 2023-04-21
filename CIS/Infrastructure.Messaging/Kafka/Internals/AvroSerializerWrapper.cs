﻿using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

internal sealed class AvroSerializerWrapper<T> : ISerializerWrapper
{
    private readonly AvroSerializer<T> _inner;

    public AvroSerializerWrapper(AvroSerializer<T> inner)
    {
        _inner = inner;
    }

    public async Task<byte[]> SerializeAsync(object data, SerializationContext context)
    {
        return await _inner.SerializeAsync((T)data, context).ConfigureAwait(false);
    }
}