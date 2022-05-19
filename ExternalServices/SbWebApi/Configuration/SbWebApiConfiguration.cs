using CIS.Foms.Enums;

namespace ExternalServices.SbWebApi.Configuration;

public sealed class SbWebApiConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBaseConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}
