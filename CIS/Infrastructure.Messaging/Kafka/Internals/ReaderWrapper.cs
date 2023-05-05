using Avro.IO;
using Avro.Specific;
using CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

internal sealed class ReaderWrapper<T> : IReaderWrapper
{
    private readonly SpecificReader<T> _reader;

    public ReaderWrapper(Avro.Schema writerSchema, Avro.Schema readerSchema)
    {
        _reader = new SpecificReader<T>(writerSchema, readerSchema);
    }

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.
    public object Read(BinaryDecoder decoder) => _reader.Read(default, decoder);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8603 // Possible null reference return.
}