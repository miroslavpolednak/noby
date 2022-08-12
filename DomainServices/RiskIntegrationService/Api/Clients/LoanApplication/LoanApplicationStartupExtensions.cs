using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class LoanApplicationStartupExtensions
{
    internal const string ServiceName = "C4MLoanApplication";

    public static WebApplicationBuilder AddLoanApplication(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<LoanApplication.Configuration.LoanApplicationConfiguration>(ServiceName);

        foreach (var configuration in configurations)
        {
            switch (configuration.Version)
            {
                case Versions.V1:
                    if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                        builder.Services.AddScoped<LoanApplication.V1.ILoanApplicationClient, LoanApplication.V1.MockLoanApplicationClient>();
                    else
                        builder.Services
                            .AddC4mHttpClient<LoanApplication.V1.ILoanApplicationClient, LoanApplication.V1.RealLoanApplicationClient>(configuration)
                            .ConfigureC4mHttpMessageHandler<LoanApplication.V1.RealLoanApplicationClient>(ServiceName)
                            .AddC4mPolicyHandler<LoanApplication.V1.ILoanApplicationClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        }

        return builder;
    }
}
