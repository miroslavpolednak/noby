using Avro;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeAvroConfig
{
    private readonly MultipleTypeAvroInfo[] _types;

    internal MultipleTypeAvroConfig(MultipleTypeAvroInfo[] types)
    {
        _types = types;
    }
        
    public IReaderWrapper CreateReader(Schema writerSchema)
    {
        var type = _types.SingleOrDefault(x => x.Schema.Fullname == writerSchema.Fullname);
        if (type == null)
        {
            throw new ArgumentException($"Unexpected type {writerSchema.Fullname}. Supported types need to be added to this {nameof(MultipleTypeAvroConfig)} instance", nameof(writerSchema));
        }
        return type.CreateReader(type.MessageType, writerSchema);
    }

    public IEnumerable<MultipleTypeAvroInfo> Types => _types.AsEnumerable();
}