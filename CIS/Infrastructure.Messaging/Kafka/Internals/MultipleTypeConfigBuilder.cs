using Avro;

namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeConfigBuilder<TBase>
{
    private readonly List<MultipleTypeInfo> _types = new();
    
    public MultipleTypeConfigBuilder<TBase> AddType(Type messageType, Schema readerSchema)
    {
        if (readerSchema is null)
        {
            throw new ArgumentNullException(nameof(readerSchema));
        }

        if (_types.Any(x => x.Schema.Fullname == readerSchema.Fullname))
        {
            throw new ArgumentException($"A type based on schema with the full name \"{readerSchema.Fullname}\" has already been added");
        }
        var mapping = new MultipleTypeInfo(messageType, readerSchema);
        _types.Add(mapping);
        return this;
    }

    public MultipleTypeConfig Build() => new(_types.ToArray());
}