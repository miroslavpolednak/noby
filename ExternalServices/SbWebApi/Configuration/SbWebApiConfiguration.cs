namespace ExternalServices.SbWebApi.Configuration;

public sealed class SbWebApiConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBaseConfiguration
{
    public override string GetVersion() => this.Version.ToString();

    public Versions Version { get; set; } = Versions.Unknown;
}
