using Microsoft.Extensions.DependencyInjection;
using CIS.InternalServices.ServiceDiscovery.Abstraction;

namespace ExternalServices.CustomerManagement;

public static class StartupExtensions
{
    public static IServiceCollection AddExternalServiceCustomerManagement(this IServiceCollection services, CMConfiguration? configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration), "CustomerManagement configuration not set");
        if (!configuration.UseServiceDiscovery && string.IsNullOrEmpty(configuration.ServiceUrl))
            throw new ArgumentNullException("ServiceUrl", "CustomerManagement Service URL must be defined");
        if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new ArgumentException("ImplementationType", "CustomerManagement Service client Implementation type is not set");

        services.AddSingleton(provider =>
        {
            // pokud se ma hledat URL v service discovery
            if (configuration.UseServiceDiscovery)
            {
                string? url = provider
                .GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetServiceUrlSynchronously(new("ES:CustomerManagement"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                configuration.ServiceUrl = url ?? throw new ArgumentNullException("url", "Service Discovery can not find ES:CustomerManagement Proprietary service URL");
            }
            return configuration;
        });

        switch (configuration.Version)
        {
            case Versions.V1:
                if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    services.AddScoped<V1.ICMClient, V1.MockCMClient>();
                else
                    services.AddHttpClient<V1.ICMClient, V1.RealCMClient>(c =>
                    {
                        c.BaseAddress = new Uri(configuration.ServiceUrl);
                    });
                break;

            default:
                throw new NotImplementedException($"CustomerManagement version {configuration.Version} client not implemented");
        }

        return services;
    }
}
