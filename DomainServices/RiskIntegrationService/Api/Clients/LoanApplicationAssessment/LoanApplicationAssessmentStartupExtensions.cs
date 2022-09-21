using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment;
using DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class LoanApplicationAssessmentStartupExtensions
{
    internal const string ServiceName = "C4MLoanApplicationAssessment";

    public static WebApplicationBuilder AddLoanApplicationAssessmentClient(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<LoanApplicationAssessmentConfiguration>(ServiceName);

        foreach (var configuration in configurations)
        {
            switch (configuration.Version, configuration.ImplementationType)
            {
                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Mock):
                    builder.Services.AddScoped<LoanApplicationAssessment.V1.ILoanApplicationAssessmentClient, LoanApplicationAssessment.V1.MockLoanApplicationAssessmentClient>();
                    break;

                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Real):
                    builder.Services
                        .AddC4mHttpClient<LoanApplicationAssessment.V1.ILoanApplicationAssessmentClient, LoanApplicationAssessment.V1.RealLoanApplicationAssessmentClient, LoanApplicationAssessmentConfiguration>(Versions.V1.ToString())
                        .ConfigureC4mHttpMessageHandler<LoanApplicationAssessment.V1.RealLoanApplicationAssessmentClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        }

        return builder;
    }
}