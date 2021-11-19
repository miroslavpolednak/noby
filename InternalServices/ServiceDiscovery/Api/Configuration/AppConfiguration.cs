using CIS.Infrastructure.Caching;

namespace CIS.InternalServices.ServiceDiscovery.Api;

public sealed class AppConfiguration
{
    public CacheTypes CacheType { get; set; }

    public string? CacheConnectionString { get; set; }
}
