namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.Configuration;

internal sealed class CreditWorthinessConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public override string GetVersion() => this.Version.ToString();

    public Versions Version { get; set; } = Versions.Unknown;
}
