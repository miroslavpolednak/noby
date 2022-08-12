namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.Configuration;

internal sealed class LoanApplicationConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}