using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.EasSimulationHT;

public static class StartupExtensions
{
    public static IServiceCollection AddExternalServiceEasSimulationHT(this IServiceCollection services, EasSimulationHTConfiguration? easSimulationHtConfiguration)
    {
        if (easSimulationHtConfiguration == null)
            throw new ArgumentNullException(nameof(easSimulationHtConfiguration), "EasSimulationHT configuration not set");
        if (!easSimulationHtConfiguration.UseServiceDiscovery && string.IsNullOrEmpty(easSimulationHtConfiguration.ServiceUrl))
            throw new ArgumentNullException("ServiceUrl", "EasSimulationHT Service URL must be defined");
        if (easSimulationHtConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new ArgumentException("ImplementationType", "Service client Implementation type is not set");

        switch (easSimulationHtConfiguration.Version)
        {
            case Versions.V6:
                if (easSimulationHtConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    services.AddScoped<V6.IEasSimulationHTClient, V6.MockEasSimulationHTClient>();
                else
                    services.AddScoped<V6.IEasSimulationHTClient, V6.RealEasSimulationHTClient>();
                break;

            default:
                throw new NotImplementedException($"EasSimulationHT version {easSimulationHtConfiguration.Version} client not implemented");
        }

        services.AddSingleton(provider =>
        {
            // pokud se ma hledat URL v service discovery
            if (easSimulationHtConfiguration.UseServiceDiscovery)
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetServiceUrlSynchronously(new("ES:EasSimulationHT"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                easSimulationHtConfiguration.ServiceUrl = url ?? throw new ArgumentNullException("url", "Service Discovery can not find ES:EasSimulationHT Proprietary service URL");
            }
            return easSimulationHtConfiguration;
        });

        return services;
    }
}
