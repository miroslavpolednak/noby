namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.Configuration;

internal sealed class LoanApplicationAssessmentConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public override string GetVersion() => this.Version.ToString();

    public Versions Version { get; set; } = Versions.Unknown;
}
