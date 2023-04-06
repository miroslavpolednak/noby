using Avro;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeAvroConfigBuilder<TBase>
{
    private readonly List<MultipleTypeAvroInfo> _types = new();
    
    public MultipleTypeAvroConfigBuilder<TBase> AddType(Type messageType, Schema readerSchema)
    {
        if (readerSchema is null)
        {
            throw new ArgumentNullException(nameof(readerSchema));
        }

        if (_types.Any(x => x.Schema.Fullname == readerSchema.Fullname))
        {
            throw new ArgumentException($"A type based on schema with the full name \"{readerSchema.Fullname}\" has already been added");
        }
        var mapping = new MultipleTypeAvroInfo(messageType, readerSchema);
        _types.Add(mapping);
        return this;
    }

    public MultipleTypeAvroConfig Build() => new(_types.ToArray());
}