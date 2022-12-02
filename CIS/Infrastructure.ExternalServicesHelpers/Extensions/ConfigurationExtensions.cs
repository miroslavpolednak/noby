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
    /// <param name="serviceName">Název konzumované služby třetí strany</param>
    /// <param name="serviceImplementationVersion">Verze proxy nad API třetí strany</param>
    /// <exception cref="CisConfigurationException">Chyba v konfiguraci služby - např. špatně zadaný typ implementace.</exception>
    /// <exception cref="CisConfigurationNotFound">Konfigurace typu TConfiguration pro klíč ES:{serviceName}:{serviceImplementationVersion} nebyla nalezena v sekci ExternalServices v appsettings.json</exception>
    public static IExternalServiceConfiguration<TClient> AddExternalServiceConfiguration<TClient>(
        this WebApplicationBuilder builder,
        string serviceName,
        string serviceImplementationVersion)
        where TClient : class, IExternalServiceClient
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        var configuration = (ExternalServiceConfiguration<TClient>)Activator.CreateInstance(typeof(ExternalServiceConfiguration<TClient>));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // zkusit nacist konfiguraci pro danou verzi
        var section = builder.Configuration.GetSection(getSectionName(serviceName, serviceImplementationVersion));
        // konfigurace pro konkretni verzi neexistuje, zkus obecnou konfiguraci
        if (!section.Exists())
            section = builder.Configuration.GetSection(getSectionName(serviceName));
        section.Bind(configuration);

        // validace konfigurace
        validateConfiguration(configuration, serviceName, serviceImplementationVersion);

        if (configuration!.UseServiceDiscovery)
        {
            builder.Services.AddSingleton<IExternalServiceConfiguration<TClient>>(provider =>
            {
                configuration.ServiceName = serviceName;
                configuration.ServiceImplementationVersion = serviceImplementationVersion;

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

        return configuration;
    }

    /// <summary>
    /// Validace konfigurace
    /// </summary>
    private static void validateConfiguration<TClient>(ExternalServiceConfiguration<TClient>? configuration, string serviceName, string serviceImplementationVersion)
        where TClient : class, IExternalServiceClient
    {
        if (configuration == null)
            throw new CisConfigurationNotFound(getSectionName(serviceName, serviceImplementationVersion));
        if (!configuration.UseServiceDiscovery && string.IsNullOrEmpty(configuration.ServiceUrl))
            throw new CisConfigurationException(0, $"{serviceName} Service URL must be defined");
        if (configuration.ImplementationType == Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");
    }

    private static string getSectionName(string serviceName, string serviceImplementationVersion)
        => $"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}:{serviceImplementationVersion}";

    private static string getSectionName(string serviceName)
        => $"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}";
}
