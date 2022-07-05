using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.Eas;

public static class StartupExtensions
{
    public static IServiceCollection AddExternalServiceEas(this IServiceCollection services, EasConfiguration? easConfiguration)
    {
        if (easConfiguration == null)
            throw new ArgumentNullException(nameof(easConfiguration), "EAS configuration not set");
        if (!easConfiguration.UseServiceDiscovery && string.IsNullOrEmpty(easConfiguration.ServiceUrl))
            throw new ArgumentNullException("ServiceUrl", "EAS Service URL must be defined");
        if (easConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new ArgumentException("ImplementationType", "Service client Implementation type is not set");

        switch (easConfiguration.Version)
        {
            case Versions.R21:
                if (easConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    services.AddScoped<R21.IEasClient, R21.MockEasClient>();
                else
                    services.AddScoped<R21.IEasClient, R21.RealEasClient>();
                break;

            default:
                throw new NotImplementedException($"EAS version {easConfiguration.Version} client not implemented");
        }

        services.AddSingleton(provider =>
        {
            // pokud se ma hledat URL v service discovery
            if (easConfiguration.UseServiceDiscovery)
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetServiceUrlSynchronously(new("ES:EAS"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc);
                easConfiguration.ServiceUrl = url ?? throw new ArgumentNullException("url", "Service Discovery can not find ES:EAS Proprietary service URL");
            }
            return easConfiguration;
        });

        return services;
    }
}
