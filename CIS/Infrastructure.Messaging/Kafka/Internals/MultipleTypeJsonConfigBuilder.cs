namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeJsonConfigBuilder<TBase>
{
    private readonly List<MultipleTypeJsonInfo> _types = new();
    
    public MultipleTypeJsonConfigBuilder<TBase> AddType(Type messageType, string payloadId)
    {
        if (string.IsNullOrEmpty(payloadId))
        {
            throw new ArgumentNullException(nameof(payloadId));
        }
        
        if (_types.Any(x => x.MessageType == messageType))
        {
            throw new ArgumentException($"A type based on schema with the type \"{messageType}\" has already been added");
        }
        var mapping = new MultipleTypeJsonInfo(messageType, payloadId);
        _types.Add(mapping);
        return this;
    }

    public MultipleTypeJsonConfig Build() => new(_types.ToArray());
}