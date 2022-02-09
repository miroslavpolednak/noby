using CIS.Core.Enums;

namespace ExternalServices.Eas;

public sealed class EasConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;

    public string ServiceUrl { get; set; } = "";

    public bool UseServiceDiscovery { get; set; } = false;

    public int Timeout { get; set; } = 5;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}
