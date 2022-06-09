using CIS.Foms.Enums;

namespace ExternalServices.Rip;

public sealed class RipConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBaseConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int Timeout { get; set; } = 5;
}
