using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness;
using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class CreditWorthinessStartupExtensions
{
    internal const string ServiceName = "C4MCreditWorthiness";

    public static WebApplicationBuilder AddCreditWorthinessClient(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<CreditWorthinessConfiguration>(ServiceName);

        configurations.ForEach(configuration =>
        {
            switch (configuration.Version, configuration.ImplementationType)
            {
                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Mock):
                    builder.Services.AddScoped<CreditWorthiness.V1.ICreditWorthinessClient, CreditWorthiness.V1.MockCreditWorthinessClient>();
                    break;

                case (Versions.V1, CIS.Foms.Enums.ServiceImplementationTypes.Real):
                    builder.Services
                        .AddC4mHttpClient<CreditWorthiness.V1.ICreditWorthinessClient, CreditWorthiness.V1.RealCreditWorthinessClient, CreditWorthinessConfiguration>(Versions.V1.ToString())
                        .ConfigureC4mHttpMessageHandler<CreditWorthiness.V1.RealCreditWorthinessClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        });

        return builder;
    }
}
