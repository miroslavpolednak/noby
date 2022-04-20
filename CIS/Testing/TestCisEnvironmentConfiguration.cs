using CIS.Core.Configuration;

namespace CIS.Testing;

public sealed class TestCisEnvironmentConfiguration : Core.Configuration.ICisEnvironmentConfiguration
{
    public string? DefaultApplicationKey { get; set; }

    public string? EnvironmentName { get; set; }

    public string? ServiceDiscoveryUrl { get; set; }

    public string? InternalServicesLogin { get; set; }

    public string? InternalServicePassword { get; set; }

    public ICisEnvironmentDistributedCacheConfiguration? DistributedCache => throw new NotImplementedException();
}
