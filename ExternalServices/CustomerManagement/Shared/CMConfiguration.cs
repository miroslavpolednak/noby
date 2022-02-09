using CIS.Core.Enums;

namespace ExternalServices.CustomerManagement;

public sealed class CMConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;

    public string ServiceUrl { get; set; } = "";

    public bool UseServiceDiscovery { get; set; } = false;

    public int Timeout { get; set; } = 5;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}