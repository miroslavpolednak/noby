using CIS.Core;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using __Client = CIS.InternalServices.ServiceDiscovery.Clients;
using __Contracts = CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices;

public static class StartupExtensions
{
    /// <summary>
    /// service locator pattern -> potrebuji pro UseServiceDiscovery
    /// </summary>
    private static IServiceCollection? _serviceCollection;

    /// <summary>
    /// Metoda doplňuje do konfigurací v DI URI dotažené ze ServiceDiscovery.
    /// </summary>
    /// <remarks>
    /// Hledá singleton instance v DI, které implementují interface IIsServiceDiscoverable. Pokud takové najde, pokusí se k nim ze SD zjistit URI a doplnit je.
    /// </remarks>
    /// <exception cref="CisArgumentNullException">Do not call UseServiceDiscovery() unless AddCisServiceDiscovery() has been called before. nebo Service Discovery can not find service URL.</exception>
    public static WebApplication UseServiceDiscovery(this WebApplication builder)
    {
        if (_serviceCollection is null)
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            throw new CisArgumentNullException(0, "Do not call UseServiceDiscovery() unless AddCisServiceDiscovery() has been called before.", nameof(_serviceCollection));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

            // najit vsechny implementace, ktere maji tento interface
        var foundServices = _serviceCollection!
            .Where(t => t.Lifetime == ServiceLifetime.Singleton && t.ImplementationInstance is not null && t.ImplementationInstance is Core.IIsServiceDiscoverable)
            .Select(t => t.ImplementationInstance as Core.IIsServiceDiscoverable)
            .Where(t => t is not null && t.UseServiceDiscovery)
            .ToList();

        if (foundServices.Any())
        {
             var servicesInServiceDiscovery = builder.Services
                .GetRequiredService<__Client.IDiscoveryServiceClient>()
                .GetServicesSynchronously();

            foundServices.ForEach(instance =>
            {
                var service = servicesInServiceDiscovery.FirstOrDefault(t => t.ServiceName == instance!.ServiceName && t.ServiceType == (__Contracts.ServiceTypes)instance.ServiceType);

                // nastavit URL ze ServiceDiscovery
                instance!.ServiceUrl = new Uri(service?.ServiceUrl
                    ?? throw new CisArgumentNullException(0, $"Service Discovery can not find {instance.ServiceName} {(__Contracts.ServiceTypes)instance.ServiceType} service URL", nameof(instance)));
            });
        }

        return builder;
    }

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, bool validateServiceCertificate = false)
    {
        if (services.AlreadyRegistered<IGrpcServiceUriSettings<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>>())
            return services;

        return services
            .AddSingleton<IGrpcServiceUriSettings<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>>(provider =>
            {
                string url = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>()?.ServiceDiscoveryUrl ?? "";
                return new GrpcServiceUriSettingsDirect<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>(url);
            })
            .registerServices(validateServiceCertificate);
    }

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, string serviceUrl, bool validateServiceCertificate = false)
    {
        if (services.AlreadyRegistered<IGrpcServiceUriSettings<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>>())
            return services;

        return services
            .AddSingleton<IGrpcServiceUriSettings<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>>(new GrpcServiceUriSettingsDirect<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>(serviceUrl))
            .registerServices(validateServiceCertificate);
    }
    
    private static IServiceCollection registerServices(this IServiceCollection services, bool validateServiceCertificate)
    {
        _serviceCollection = services;

        // cache
        services.AddTransient<__Client.ServicesMemoryCache>();
        
        // abstraction svc
        services.AddTransient<__Client.IDiscoveryServiceClient, __Client.DiscoveryServiceClient>();

        // def environment name
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>();
            return new __Client.EnvironmentNameProvider(configuration?.EnvironmentName);
        });

        // napojeni na gRPC
        services.AddCisGrpcClientInner<__Contracts.v1.DiscoveryService.DiscoveryServiceClient, __Contracts.v1.DiscoveryService.DiscoveryServiceClient>(validateServiceCertificate, false);

        return services;
    }
}
