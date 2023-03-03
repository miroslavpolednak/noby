using Avro;
using Avro.Specific;

namespace Mock.Mcs.Messaging.Infrastructure;

public class MultipleTypeConfigBuilder<TBase>
{
    private readonly List<MultipleTypeInfo> _types = new();
    
    public MultipleTypeConfigBuilder<TBase> AddType<T>(Schema readerSchema)
        where T : TBase, ISpecificRecord
    {
        if (readerSchema is null)
        {
            throw new ArgumentNullException(nameof(readerSchema));
        }

        if (_types.Any(x => x.Schema.Fullname == readerSchema.Fullname))
        {
            throw new ArgumentException($"A type based on schema with the full name \"{readerSchema.Fullname}\" has already been added");
        }
        var messageType = typeof(T);
        var mapping = new MultipleTypeInfo<T>(messageType, readerSchema);
        _types.Add(mapping);
        return this;
    }

    public MultipleTypeConfig Build() => new(_types.ToArray());
}