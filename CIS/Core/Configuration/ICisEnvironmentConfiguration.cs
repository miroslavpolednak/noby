namespace CIS.Core.Configuration;

public interface ICisEnvironmentConfiguration
{
    string? DefaultApplicationKey { get; }

    string? EnvironmentName { get; }

    string? ServiceDiscoveryUrl { get; }

    string? InternalServicesLogin { get; }

    string? InternalServicePassword { get; }
}
