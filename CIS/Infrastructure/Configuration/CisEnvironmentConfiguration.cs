namespace CIS.Infrastructure.Configuration;

public sealed class CisEnvironmentConfiguration
    : Core.Configuration.ICisEnvironmentConfiguration
{
    public string? DefaultApplicationKey { get; set; }

    public string? EnvironmentName { get; set; }

    public string? ServiceDiscoveryUrl { get; set; }

    public string? InternalServicesLogin { get; set; }

    public string? InternalServicePassword { get; set; }
}
