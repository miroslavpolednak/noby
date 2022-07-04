using CIS.ExternalServicesHelpers;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.C4M.CreditWorthiness;

public static class StartupExtensions
{
    internal const string ServiceName = "C4MCreditWorthiness";

    public static WebApplicationBuilder AddExternalServiceC4MCreditWorthiness(this WebApplicationBuilder builder)
    {
        var configuration = builder.CreateAndCheckExternalServiceConfiguration<Configuration.CreditWorthinessConfiguration>(ServiceName);

        switch (configuration.Version)
        {
            case Versions.V1:
                if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    builder.Services.AddScoped<V1.ICreditWorthinessClient, V1.MockCreditWorthinessClient>();
                else
                    builder.Services.AddHttpClient<V1.ICreditWorthinessClient, V1.RealCreditWorthinessClient>((services, client) =>
                    {
                        // service url
                        if (configuration.UseServiceDiscovery)
                        {
                            string url = services.GetRequiredService<IDiscoveryServiceAbstraction>()
                                .GetServiceUrlSynchronously(new($"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}{ServiceName}"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                            client.BaseAddress = new Uri(url!);
                        }
                        else
                            client.BaseAddress = new Uri(configuration.ServiceUrl);
                    });
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
        }

        return builder;
    }


}
