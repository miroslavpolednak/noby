﻿using Avro;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeConfig
{
    private readonly MultipleTypeInfo[] _types;

    internal MultipleTypeConfig(MultipleTypeInfo[] types)
    {
        _types = types;
    }
        
    public IReaderWrapper CreateReader(Schema writerSchema)
    {
        var type = _types.SingleOrDefault(x => x.Schema.Fullname == writerSchema.Fullname);
        if (type == null)
        {
            throw new ArgumentException($"Unexpected type {writerSchema.Fullname}. Supported types need to be added to this {nameof(MultipleTypeConfig)} instance", nameof(writerSchema));
        }
        return type.CreateReader(type.MessageType, writerSchema);
    }

    public IEnumerable<MultipleTypeInfo> Types => _types.AsEnumerable();
}