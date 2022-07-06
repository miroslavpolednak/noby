using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

public static class ServiceDiscoveryExtensions
{
    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services)
        => services.TryAddGrpcClient<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(a => 
                a.registerUriSettings()
                .registerServices()
            );

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, string discoveryServiceUrl)
        => services.TryAddGrpcClient<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(a =>
            a.AddGrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(discoveryServiceUrl)
            .registerServices()
        );

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
        => services.AddSingleton(provider =>
        {
            string url = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>()?.ServiceDiscoveryUrl ?? "";
            var logger = provider.GetService<ILoggerFactory>();
            if (logger != null) logger.CreateLogger<GrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>>().DiscoveryServiceUrlFound(url);
            return new GrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(url);
        });

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // cache
        services.AddTransient<ServicesMemoryCache>();
        // abstraction svc
        services.AddTransient<IDiscoveryServiceAbstraction, DiscoveryService>();
        // def environment name
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>();
            return new EnvironmentNameProvider(configuration?.EnvironmentName);
        });

        // register service client
        services.AddGrpcClientFromCisEnvironment<Contracts.v1.DiscoveryService.DiscoveryServiceClient>();

        return services;
    }
}
