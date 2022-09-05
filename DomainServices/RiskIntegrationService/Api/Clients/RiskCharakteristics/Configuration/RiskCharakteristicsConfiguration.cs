namespace DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics.Configuration;

internal sealed class RiskCharakteristicsConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}
