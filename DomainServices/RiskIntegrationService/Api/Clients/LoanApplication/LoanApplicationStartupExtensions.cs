using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication;
using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class LoanApplicationStartupExtensions
{
    internal const string ServiceName = "C4MLoanApplication";

    public static WebApplicationBuilder AddLoanApplicationClient(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<LoanApplicationConfiguration>(ServiceName);

        configurations.ForEach(configuration =>
        {
            switch (configuration.Version, configuration.ImplementationType)
            {
                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Mock):
                    builder.Services.AddScoped<LoanApplication.V1.ILoanApplicationClient, LoanApplication.V1.MockLoanApplicationClient>();
                    break;

                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Real):
                    builder.Services
                        .AddC4mHttpClient<LoanApplication.V1.ILoanApplicationClient, LoanApplication.V1.RealLoanApplicationClient, LoanApplicationConfiguration>(Versions.V1.ToString())
                        .ConfigureC4mHttpMessageHandler<LoanApplication.V1.RealLoanApplicationClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        });

        return builder;
    }
}
