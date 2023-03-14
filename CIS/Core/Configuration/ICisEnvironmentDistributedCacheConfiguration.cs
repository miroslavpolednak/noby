namespace CIS.Core.Configuration;

public interface ICisEnvironmentDistributedCacheConfiguration
{
    string? ConnectionString { get; }

    string? KeyPrefix { get; }

    CacheTypes CacheType { get; }

    public enum CacheTypes
    {
        None = 0,
        InMemory = 1,
        Redis = 2
    }
}