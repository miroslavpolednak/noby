namespace ExternalServices.AddressWhisperer;

public sealed class AddressWhispererConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public override string GetVersion() => Version.ToString();

    public Versions Version { get; set; } = Versions.Unknown;
}
