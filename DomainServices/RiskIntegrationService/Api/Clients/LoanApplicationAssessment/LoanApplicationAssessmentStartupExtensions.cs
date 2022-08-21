using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class LoanApplicationAssessmentStartupExtensions
{
    internal const string ServiceName = "C4MLoanApplicationAssessment";

    public static WebApplicationBuilder AddLoanApplicationAssessment(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<LoanApplicationAssessment.Configuration.LoanApplicationAssessmentConfiguration>(ServiceName);

        foreach (var configuration in configurations)
        {
            switch (configuration.Version)
            {
                case Versions.V0_2:
                    if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                        builder.Services.AddScoped<LoanApplicationAssessment.V0_2.ILoanApplicationAssessmentClient, LoanApplicationAssessment.V0_2.MockLoanApplicationAssessmentClient>();
                    else
                        builder.Services
                            .AddC4mHttpClient<LoanApplicationAssessment.V0_2.ILoanApplicationAssessmentClient, LoanApplicationAssessment.V0_2.RealLoanApplicationAssessmentClient>(configuration)
                            .ConfigureC4mHttpMessageHandler<RiskBusinessCase.V0_2.RealRiskBusinessCaseClient>(ServiceName)
                            .AddC4mPolicyHandler<RiskBusinessCase.V0_2.IRiskBusinessCaseClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        }

        return builder;
    }
}