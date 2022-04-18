namespace CIS.Infrastructure.Configuration;

internal sealed class CisEnvironmentDistributedCacheConfiguration
    : CIS.Core.Configuration.ICisEnvironmentDistributedCacheConfiguration
{
    public string? ConnectionString { get; set; }

    public string? KeyPrefix { get; set; }

    public CIS.Core.Configuration.ICisEnvironmentDistributedCacheConfiguration.CacheTypes CacheType { get; set; }
}
