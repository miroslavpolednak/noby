using CIS.Foms.Enums;

namespace CIS.ExternalServicesHelpers.Configuration;

public abstract class ExternalServiceBasicAuthenticationConfiguration
    : ExternalServiceBaseConfiguration, IExternalServiceBasicAuthenticationConfiguration
{
    public string? Username { get; set; }

    public string? Password { get; set; }
}
