namespace DomainServices.RiskIntegrationService.Api.Clients.CustomersExposure.Configuration;

internal sealed class CustomersExposureConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}
