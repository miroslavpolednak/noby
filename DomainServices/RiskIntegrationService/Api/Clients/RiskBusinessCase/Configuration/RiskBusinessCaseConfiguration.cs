namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.Configuration;

internal sealed class RiskBusinessCaseConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public override string GetVersion() => this.Version.ToString();

    public Versions Version { get; set; } = Versions.Unknown;
}
