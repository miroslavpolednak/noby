namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.Configuration;

internal sealed class CreditWorthinessConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}
