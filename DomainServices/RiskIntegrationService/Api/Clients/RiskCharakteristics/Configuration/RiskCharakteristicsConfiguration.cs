namespace DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.Configuration;

internal sealed class RiskCharakteristicsConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public override string GetVersion() => this.Version.ToString();

    public Versions Version { get; set; } = Versions.Unknown;
}
