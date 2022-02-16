using CIS.Foms.Enums;

namespace ExternalServices.ESignatures;

public sealed class ESignaturesConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;

    public string ServiceUrl { get; set; } = "";

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool UseServiceDiscovery { get; set; } = false;

    public int Timeout { get; set; } = 5;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}
