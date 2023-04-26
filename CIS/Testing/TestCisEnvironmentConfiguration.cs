using CIS.Core.Configuration;

namespace CIS.Testing;

public sealed class TestCisEnvironmentConfiguration 
    : ICisEnvironmentConfiguration
{
    public string? DefaultApplicationKey { get; set; }

    public string? EnvironmentName { get; set; } = Constants.CisEnvironmentNameTest;

    public string? ServiceDiscoveryUrl { get; set; }

    public string? InternalServicesLogin { get; set; } = Constants.CisSecurityUsername;

    public string? InternalServicePassword { get; set; } = Constants.CisSecurityPassword;

    public bool DisableServiceDiscovery { get; set; } = true;
}