using CIS.Core;
using CIS.Core.Configuration;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
    /// <exception cref="CisArgumentException">Service Discovery can not find {ServiceName} {ServiceType} service URL</exception>
    public static WebApplication UseServiceDiscovery(this WebApplication builder)
    {
        if (_serviceCollection is null)
        {
            throw new CisArgumentException(0, "Do not call UseServiceDiscovery() unless AddCisServiceDiscovery() has been called before.", nameof(_serviceCollection));
        }

        // konfigurace prostredi - kvuli tomu, kdybysme chteli vypnout SD
        var configuration = builder.Services.GetRequiredService<ICisEnvironmentConfiguration>();
        
        // najit vsechny implementace, ktere maji tento interface
        var foundServices = _serviceCollection!
            .Where(t => 
                t.Lifetime == ServiceLifetime.Singleton 
                && t.ImplementationInstance is not null 
                && t.ImplementationInstance is IIsServiceDiscoverable)
            .Select(t => t.ImplementationInstance as IIsServiceDiscoverable)
            .Where(t => t is not null && t.UseServiceDiscovery)
            .ToList();

        // nejsou zadne auto discover sluzby
        if (!foundServices.Any())
        {
            return builder;
        }

        // vypnuta SD, stejne musim nastavit URL - pro testovani
        if (configuration.DisableServiceDiscovery)
        {
            foundServices.ForEach(instance =>
            {
                instance!.ServiceUrl = new Uri(GrpcServiceUriSettingsConstants.EmptyUriAddress);
            });
        }
        else // regulerni beh
        {
            var servicesInServiceDiscovery = builder.Services
                .GetRequiredService<__Client.IDiscoveryServiceClient>()
                .GetServicesSynchronously();

            foundServices.ForEach(instance =>
            {
                var service = servicesInServiceDiscovery
                    .FirstOrDefault(t => t.ServiceName == instance!.ServiceName && t.ServiceType == (__Contracts.ServiceTypes)instance.ServiceType);

                // nastavit URL ze ServiceDiscovery
                if (Uri.TryCreate(service?.ServiceUrl, UriKind.Absolute, out var serviceUri))
                {
                    instance!.ServiceUrl = serviceUri;
                }
                else
                {
                    throw new CisArgumentException(0, $"Service Discovery can not find {instance!.ServiceName} {(__Contracts.ServiceTypes)instance.ServiceType} service URL", nameof(instance));
                }
            });
        }

        return builder;
    }

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, bool validateServiceCertificate = false)
    {
        // najit instanci konfigurace
        var configuration = services
            .Where(t =>
                t.Lifetime == ServiceLifetime.Singleton
                && t.ImplementationInstance is not null
                && t.ImplementationInstance is ICisEnvironmentConfiguration)
            .Select(t => t.ImplementationInstance as ICisEnvironmentConfiguration)
            .FirstOrDefault();

        // pokud se nema pouzivat SD, tak registrovat prazdny settings
        if (configuration?.DisableServiceDiscovery ?? false)
        {
            services.TryAddSingleton<IGrpcServiceUriSettings<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>, GrpcServiceUriSettingsEmpty<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>>();
        }
        else
        {
            services.TryAddSingleton<IGrpcServiceUriSettings<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>>(provider =>
            {
                var configuration = provider.GetService<ICisEnvironmentConfiguration>();

                string url = provider.GetService<ICisEnvironmentConfiguration>()?.ServiceDiscoveryUrl ?? "";
                return new GrpcServiceUriSettingsDirect<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>(url);
            });
            services.registerServices(validateServiceCertificate);
        }

        _serviceCollection ??= services;

        return services;
    }

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, string serviceUrl, bool validateServiceCertificate = false)
    {
        services.TryAddSingleton<IGrpcServiceUriSettings<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>>(new GrpcServiceUriSettingsDirect<__Contracts.v1.DiscoveryService.DiscoveryServiceClient>(serviceUrl));
        
        _serviceCollection ??= services;
        services.registerServices(validateServiceCertificate);
        
        return services;
    }
    
    private static IServiceCollection registerServices(this IServiceCollection services, bool validateServiceCertificate)
    {
        // cache
        services.TryAddTransient<__Client.ServicesMemoryCache>();
        
        // abstraction svc
        services.TryAddTransient<__Client.IDiscoveryServiceClient, __Client.DiscoveryServiceClient>();

        // def environment name
        services.TryAddSingleton(provider =>
        {
            var configuration = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>();
            return new __Client.EnvironmentNameProvider(configuration?.EnvironmentName);
        });

        // napojeni na gRPC
        services.AddCisGrpcClientInner<__Contracts.v1.DiscoveryService.DiscoveryServiceClient, __Contracts.v1.DiscoveryService.DiscoveryServiceClient>(validateServiceCertificate, false);

        return services;
    }
}
