namespace CIS.Infrastructure.Messaging.Kafka.Internals;

public sealed class MultipleTypeJsonConfig
{
    private readonly MultipleTypeJsonInfo[] _types;

    internal MultipleTypeJsonConfig(MultipleTypeJsonInfo[] types)
    {
        _types = types;
    }

    public IEnumerable<MultipleTypeJsonInfo> Types => _types.AsEnumerable();
}