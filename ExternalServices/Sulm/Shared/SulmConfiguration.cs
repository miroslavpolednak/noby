namespace ExternalServices.Sulm;

public sealed class SulmConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBaseConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}
