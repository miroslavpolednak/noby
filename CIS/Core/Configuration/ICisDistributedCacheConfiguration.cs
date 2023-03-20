namespace CIS.Core.Configuration;

public interface ICisDistributedCacheConfiguration
{
    string? KeyPrefix { get; }

    SerializationTypes SerializationType { get; }

    CacheTypes CacheType { get; }

    public enum CacheTypes
    {
        None = 0,
        InMemory = 1,
        Redis = 2,
        MsSql = 3
    }

    public enum SerializationTypes
    {
        Default = 0,
        Json = 1,
        Protobuf = 2
    }
}