namespace CIS.Infrastructure.Caching.Configuration;

internal sealed class CisDistributedCacheConfiguration
    : CIS.Core.Configuration.ICisDistributedCacheConfiguration
{
    public string? KeyPrefix { get; set; }

    public CIS.Core.Configuration.ICisDistributedCacheConfiguration.SerializationTypes SerializationType { get; set; }

    public CIS.Core.Configuration.ICisDistributedCacheConfiguration.CacheTypes CacheType { get; set; }
}
