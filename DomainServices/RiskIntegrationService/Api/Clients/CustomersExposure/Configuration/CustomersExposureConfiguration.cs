namespace DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.Configuration;

internal sealed class CustomersExposureConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public override string GetVersion() => this.Version.ToString();

    public Versions Version { get; set; } = Versions.Unknown;
}
