using CIS.Core;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.ServiceDiscovery.Clients;

public static class StartupExtensions
{
    /// <summary>
    /// service locator pattern -> potrebuji pro UseServiceDiscovery
    /// </summary>
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
                instance!.ServiceUrl = new Uri(service?.ServiceUrl
                    ?? throw new ArgumentNullException("url", $"Service Discovery can not find {instance.ServiceName} {(Contracts.ServiceTypes)instance.ServiceType} service URL"));
            });
        }

        return builder;
    }

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, bool validateServiceCertificate = false)
    {
        if (services.AlreadyRegistered<IGrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>>())
            return services;

        return services
            .AddSingleton<IGrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>>(provider =>
            {
                string url = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>()?.ServiceDiscoveryUrl ?? "";
                return new GrpcServiceUriSettingsDirect<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(url);
            })
            .registerServices(validateServiceCertificate);
    }

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, string serviceUrl, bool validateServiceCertificate = false)
    {
        if (services.AlreadyRegistered<IGrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>>())
            return services;

        return services
            .AddSingleton<IGrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>>(new GrpcServiceUriSettingsDirect<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(serviceUrl))
            .registerServices(validateServiceCertificate);
    }
    
    private static IServiceCollection registerServices(this IServiceCollection services, bool validateServiceCertificate)
    {
        _serviceCollection = services;

        // cache
        services.AddTransient<ServicesMemoryCache>();
        
        // abstraction svc
        services.AddScoped<IDiscoveryServiceAbstraction, DiscoveryService>();

        // def environment name
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>();
            return new EnvironmentNameProvider(configuration?.EnvironmentName);
        });

        // napojeni na gRPC
        services.AddCisGrpcClientInner<Contracts.v1.DiscoveryService.DiscoveryServiceClient, Contracts.v1.DiscoveryService.DiscoveryServiceClient>(validateServiceCertificate, false);

        return services;
    }
}
