using CIS.Foms.Enums;

namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

public class ExternalServiceConfiguration<TClient>
    : IExternalServiceConfiguration<TClient>
    where TClient : class
{
    public bool LogPayloads { get; set; } = true;

    public int? RequestTimeout { get; set; } = 10;

    public string ServiceUrl { get; set; } = "";

    public bool UseServiceDiscovery { get; set; } = true;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;

    public bool PropagateKbHeaders { get; set; }
}
