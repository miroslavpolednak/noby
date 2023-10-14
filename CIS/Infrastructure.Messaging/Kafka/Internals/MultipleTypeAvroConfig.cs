using Avro;
using CIS.Infrastructure.Messaging.Exceptions;
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
            throw new KafkaMessageTypeNotSupportedException(writerSchema.Fullname);
        }
        return type.CreateReader(type.MessageType, writerSchema);
    }

    public IEnumerable<MultipleTypeAvroInfo> Types => _types.AsEnumerable();
}