using CIS.Foms.Enums;

namespace CIS.ExternalServicesHelpers.Configuration;

public abstract class ExternalServiceBaseConfiguration
    : IExternalServiceConfiguration
{
    public int? RequestTimeout { get; set; } = 10;

    public string ServiceUrl { get; set; } = "";

    public bool UseServiceDiscovery { get; set; } = true;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}
