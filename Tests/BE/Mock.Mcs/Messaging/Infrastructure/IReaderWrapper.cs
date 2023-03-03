using Avro.IO;

namespace Mock.Mcs.Messaging.Infrastructure;

public interface IReaderWrapper
{
    object Read(BinaryDecoder decoder);
}