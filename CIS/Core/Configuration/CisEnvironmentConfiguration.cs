namespace CIS.Core.Configuration;

public sealed class CisEnvironmentConfiguration
    : ICisEnvironmentConfiguration
{
    public string? DefaultApplicationKey { get; set; }

    public string? EnvironmentName { get; set; }

    public SecretsSource SecretsSource { get; set; }

    public string? ServiceDiscoveryUrl { get; set; }

    public string? InternalServicesLogin { get; set; }

    public string? InternalServicePassword { get; set; }

    public bool DisableServiceDiscovery { get; set; }

    public bool DisableContractDescriptionPropagation { get; set; }
}
