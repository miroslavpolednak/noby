using CIS.Foms.Enums;

namespace CIS.ExternalServicesHelpers.Configuration;

public interface IExternalServiceConfiguration
{
    string ServiceUrl { get; set; }

    bool UseServiceDiscovery { get; set; }

    ServiceImplementationTypes ImplementationType { get; set; }
}
