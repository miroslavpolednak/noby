using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Načtení konfigurace externí služby a její vložení do DI.
    /// </summary>
    /// <typeparam name="TClient">Typ klienta - interface pro danou verzi proxy nad API třetí strany</typeparam>
    /// <typeparam name="TConfiguration">Typ konfigurace, který bude pro tohoto TClient vložen do Di</typeparam>
    /// <param name="serviceName">Název konzumované služby třetí strany</param>
    /// <param name="serviceImplementationVersion">Verze proxy nad API třetí strany</param>
    /// <exception cref="CisConfigurationException">Chyba v konfiguraci služby - např. špatně zadaný typ implementace.</exception>
    /// <exception cref="CisConfigurationNotFound">Konfigurace typu TConfiguration pro klíč ES:{serviceName}:{serviceImplementationVersion} nebyla nalezena v sekci ExternalServices v appsettings.json</exception>
    public static TConfiguration AddExternalServiceConfiguration<TClient, TConfiguration>(
    this WebApplicationBuilder builder,
    string serviceName,
    string serviceImplementationVersion)
    where TClient : class, IExternalServiceClient
    where TConfiguration : class, IExternalServiceConfiguration<TClient>
    {
        // ziskat konfiguraci sluzby z appsettings.json
        var configuration = builder.readConfiguration<TConfiguration>(serviceName, serviceImplementationVersion);
        builder.registerConfiguration(configuration, serviceName, serviceImplementationVersion);
        return configuration;
    }

    private static void registerConfiguration<TConfiguration>(this WebApplicationBuilder builder, TConfiguration configuration, string serviceName, string serviceImplementationVersion)
        where TConfiguration : class, IExternalServiceConfiguration
    {
        if (configuration.UseServiceDiscovery)
        {
            builder.Services.AddSingleton(provider =>
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetServiceUrlSynchronously(new($"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}{serviceName}"), InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);

                // nastavit URL ze ServiceDiscovery
                configuration.ServiceUrl = url
                    ?? throw new ArgumentNullException("url", $"Service Discovery can not find {Constants.ExternalServicesConfigurationSectionName}:{serviceName}:{serviceImplementationVersion} Proprietary service URL");

                return configuration;
            });
        }
        else
            builder.Services.AddSingleton(configuration);
    }

    private static TConfiguration readConfiguration<TConfiguration>(this WebApplicationBuilder builder, string serviceName, string serviceImplementationVersion)
        where TConfiguration : class, IExternalServiceConfiguration
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        TConfiguration configuration = (TConfiguration)Activator.CreateInstance(typeof(TConfiguration));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // zkusit nacist konfiguraci pro danou verzi
        builder.Configuration.GetSection(getSectionName(serviceName, serviceImplementationVersion)).Bind(configuration);

        // konfigurace pro konkretni verzi neexistuje, zkus obecnou konfiguraci
        if (configuration == null)
            builder.Configuration.GetSection($"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}").Bind(configuration);

        if (configuration == null)
            throw new CisConfigurationNotFound(getSectionName(serviceName, serviceImplementationVersion));
        if (!configuration.UseServiceDiscovery && string.IsNullOrEmpty(configuration.ServiceUrl))
            throw new CisConfigurationException(0, $"{serviceName} Service URL must be defined");
        if (configuration.ImplementationType == Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");

        return configuration;
    }

    private static string getSectionName(string serviceName, string serviceImplementationVersion)
        => $"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}:{serviceImplementationVersion}";
}
