namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.Configuration;

internal sealed class RiskBusinessCaseConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}
