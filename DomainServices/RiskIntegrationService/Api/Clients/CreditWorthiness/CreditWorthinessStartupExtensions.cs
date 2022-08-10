using CIS.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class CreditWorthinessStartupExtensions
{
    internal const string ServiceName = "C4MCreditWorthiness";

    public static WebApplicationBuilder AddCreditWorthiness(this WebApplicationBuilder builder)
    {
        var configurations = builder.CreateAndCheckExternalServiceConfigurationsList<CreditWorthiness.Configuration.CreditWorthinessConfiguration>(ServiceName);

        foreach (var configuration in configurations)
        {
            switch (configuration.Version)
            {
                case Versions.V1:
                    if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                        builder.Services.AddScoped<CreditWorthiness.V1.ICreditWorthinessClient, CreditWorthiness.V1.MockCreditWorthinessClient>();
                    else
                        builder.Services
                            .AddC4mHttpClient<CreditWorthiness.V1.ICreditWorthinessClient, CreditWorthiness.V1.RealCreditWorthinessClient>(configuration)
                            .ConfigureC4mHttpMessageHandler<CreditWorthiness.V1.RealCreditWorthinessClient>(ServiceName)
                            .AddC4mPolicyHandler<CreditWorthiness.V1.ICreditWorthinessClient>(ServiceName);
                    break;

                default:
                    throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
            }
        }

        return builder;
    }
}
