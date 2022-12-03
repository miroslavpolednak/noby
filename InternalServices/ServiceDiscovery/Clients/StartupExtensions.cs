using CIS.Infrastructure.gRPC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.ServiceDiscovery.Clients;

public static class StartupExtensions
{
    private static IServiceCollection? _serviceCollection;

    public static WebApplication UseServiceDiscovery(this WebApplication builder)
    {
        // najit vsechny implementace, ktere maji tento interface
        var tIsDiscoverable = typeof(Core.IIsServiceDiscoverable);
        var foundServices = _serviceCollection!
            .Where(t => t.ServiceType.IsAssignableTo(tIsDiscoverable))
            .Select(t => t.ImplementationInstance as Core.IIsServiceDiscoverable)
            .Where(t => t is not null && t.UseServiceDiscovery)
            .ToList();

        if (foundServices.Any())
        {
             var servicesInServiceDiscovery = builder.Services
                .GetRequiredService<IDiscoveryServiceAbstraction>()
                .GetServicesSynchronously();

            foundServices.ForEach(instance =>
            {
                var service = servicesInServiceDiscovery.FirstOrDefault(t => t.ServiceName == instance!.ServiceName && t.ServiceType == (Contracts.ServiceTypes)instance.ServiceType);

                // nastavit URL ze ServiceDiscovery
                instance!.ServiceUrl = service?.ServiceUrl
                    ?? throw new ArgumentNullException("url", $"Service Discovery can not find {instance.ServiceName} {(Contracts.ServiceTypes)instance.ServiceType} service URL");
            });
        }

        return builder;
    }

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
            return new GrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(url);
        });

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        _serviceCollection = services;

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
