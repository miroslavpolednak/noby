using Avro.IO;

namespace CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;

public interface IReaderWrapper
{
    object Read(BinaryDecoder decoder);
}