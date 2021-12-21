namespace CIS.Infrastructure.Configuration;

public class CacheConfiguration
{
    public Caching.CacheTypes CacheType { get; set; }

    public string? CacheConnectionString { get; set; }

    public string? CacheKeyPrefix { get; set; }

    public bool UseServiceDiscovery { get; set; }
}
