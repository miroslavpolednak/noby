namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.Configuration;

internal sealed class LoanApplicationAssessmentConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBasicAuthenticationConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}
