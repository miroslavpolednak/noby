namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.Configuration;

internal sealed class LoanApplicationConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public override string GetVersion() => this.Version.ToString();

    public Versions Version { get; set; } = Versions.Unknown;
}