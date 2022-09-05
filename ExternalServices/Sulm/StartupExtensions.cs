using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.Sulm;

public static class StartupExtensions
{
    public static IServiceCollection AddExternalServiceSulm(this IServiceCollection services, SulmConfiguration? sulmConfiguration)
    {
        if (sulmConfiguration == null)
            throw new ArgumentNullException(nameof(sulmConfiguration), "SULM configuration not set");
        if (!sulmConfiguration.UseServiceDiscovery && string.IsNullOrEmpty(sulmConfiguration.ServiceUrl))
            throw new ArgumentNullException("ServiceUrl", "SULM Service URL must be defined");
        if (sulmConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new ArgumentException("ImplementationType", "Service client Implementation type is not set");

        switch (sulmConfiguration.Version)
        {
            case Versions.V1:
                if (sulmConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    services.AddScoped<V1.ISulmClient, V1.MockSulmClient>();
                else
                    services.AddScoped<V1.ISulmClient, V1.RealSulmClient>();
                break;

            default:
                throw new NotImplementedException($"SULM version {sulmConfiguration.Version} client not implemented");
        }

        services.AddSingleton(provider =>
        {
            // pokud se ma hledat URL v service discovery
            if (sulmConfiguration.UseServiceDiscovery)
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetServiceUrlSynchronously(new("ES:SULM"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                sulmConfiguration.ServiceUrl = url ?? throw new ArgumentNullException("url", "Service Discovery can not find ES:SULM Proprietary service URL");
            }
            return sulmConfiguration;
        });

        return services;
    }
}
