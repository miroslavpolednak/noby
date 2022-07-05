using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

public static class ServiceDiscoveryExtensions
{
    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services)
        => services
            .registerUriSettings()
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, string discoveryServiceUrl)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(discoveryServiceUrl)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string url = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>()?.ServiceDiscoveryUrl ?? "";
                var logger = provider.GetService<ILoggerFactory>();
                if (logger != null) logger.CreateLogger<GrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>>().DiscoveryServiceUrlFound(url);
                return new GrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(url);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // cache
        services.TryAddTransient<ServicesMemoryCache>();
        // abstraction svc
        services.TryAddTransient<IDiscoveryServiceAbstraction, DiscoveryService>();
        // def environment name
        services.TryAddSingleton(provider =>
        {
            var configuration = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>();
            return new EnvironmentNameProvider(configuration?.EnvironmentName);
        });

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.DiscoveryService.DiscoveryServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.DiscoveryService.DiscoveryServiceClient>();
        }
        return services;
    }
}
