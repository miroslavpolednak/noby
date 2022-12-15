using CIS.Infrastructure.Configuration;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
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
    /// <exception cref="CisConfigurationNotFound">Konfigurace pro klíč ES:{serviceName}:{serviceImplementationVersion} nebyla nalezena v sekci ExternalServices v appsettings.json</exception>
    public static IExternalServiceConfiguration<TClient> AddExternalServiceConfiguration<TClient>(
        this WebApplicationBuilder builder,
        string serviceName,
        string serviceImplementationVersion)
        where TClient : class, IExternalServiceClient
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        var configuration = (ExternalServiceConfiguration<TClient>)Activator.CreateInstance(typeof(ExternalServiceConfiguration<TClient>));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        builder.Configuration
            .GetSection(getSectionName(serviceName, serviceImplementationVersion))
            .Bind(configuration);

        // validace konfigurace
        if (configuration == null)
            throw new CisConfigurationNotFound(getSectionName(serviceName, serviceImplementationVersion));
        if (!configuration.UseServiceDiscovery && configuration.ServiceUrl == null)
            throw new CisConfigurationException(0, $"{serviceName} Service URL must be defined");
        if (configuration.ImplementationType == Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");

        configuration!.ServiceName = $"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}{serviceName}:{serviceImplementationVersion}";

        builder.Services.AddSingleton<IExternalServiceConfiguration<TClient>>(configuration);

        return configuration;
    }

    private static string getSectionName(in string serviceName, in string serviceImplementationVersion)
        => $"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}:{serviceImplementationVersion}";
}
