using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.RiskCharakteristics;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class RiskCharakteristicsStartupExtensions
{
    internal const string ServiceName = "C4MRiskCharakteristics";

    public static WebApplicationBuilder AddRiskCharakteristics(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<RiskCharakteristics.Configuration.RiskCharakteristicsConfiguration>(ServiceName);

        configurations.ForEach(configuration =>
        {
            switch (configuration.Version, configuration.ImplementationType)
            {
                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Mock):
                    builder.Services.AddScoped<RiskCharakteristics.V1.IRiskCharakteristicsClient, RiskCharakteristics.V1.MockRiskCharakteristicsClient>();
                    break;

                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Real):
                    builder.Services
                        .AddC4mHttpClient<RiskCharakteristics.V1.IRiskCharakteristicsClient, RiskCharakteristics.V1.RealRiskCharakteristicsClient>(configuration)
                        .ConfigureC4mHttpMessageHandler<LoanApplication.V1.RealLoanApplicationClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        });

        return builder;
    }
}
