using CIS.Core.Configuration;

namespace CIS.Testing;

public sealed class TestCisEnvironmentConfiguration 
    : Core.Configuration.ICisEnvironmentConfiguration
{
    public bool DisableServiceDiscovery { get; set; }

    public string? DefaultApplicationKey { get; set; }

    public string? EnvironmentName { get; set; }

    public string? ServiceDiscoveryUrl { get; set; }

    public string? InternalServicesLogin { get; set; }

    public string? InternalServicePassword { get; set; }

    public ICisDistributedCacheConfiguration? DistributedCache => throw new NotImplementedException();
}
