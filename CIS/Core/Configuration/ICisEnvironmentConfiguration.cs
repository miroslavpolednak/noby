namespace CIS.Core.Configuration;

public interface ICisEnvironmentConfiguration
{
    string? DefaultApplicationKey { get; set; }

    string? EnvironmentName { get; set; }

    string? ServiceDiscoveryUrl { get; set; }

    string? InternalServicesLogin { get; set; }

    string? InternalServicePassword { get; set; }
}
