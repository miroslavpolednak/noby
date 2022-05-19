using CIS.Foms.Enums;

namespace CIS.ExternalServicesHelpers.Configuration;

public abstract class ExternalServiceBaseConfiguration
    : IExternalServiceConfiguration
{
    public string ServiceUrl { get; set; } = "";

    public bool UseServiceDiscovery { get; set; } = false;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}
