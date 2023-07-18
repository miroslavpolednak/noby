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
        var configuration = (ExternalServiceConfiguration<TClient>)Activator.CreateInstance(typeof(ExternalServiceConfiguration<TClient>))!;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        builder.Configuration
            .GetSection(getSectionName(serviceName, serviceImplementationVersion))
            .Bind(configuration);

        validateConfiguration(configuration, serviceName, serviceImplementationVersion);
        addConfigurationToDi(builder.Services, configuration, serviceName, serviceImplementationVersion);

        return configuration;
    }

    /// <summary>
    /// Načtení konfigurace externí služby a její vložení do DI.
    /// Přetížení se používá pro případ, že externí služba má vlastní konfigurační třídu.
    /// </summary>
    /// <typeparam name="TClient">Typ klienta - interface pro danou verzi proxy nad API třetí strany</typeparam>
    /// <typeparam name="TConfiguration">Typ konfiguracni tridy</typeparam>
    /// <param name="serviceName">Název konzumované služby třetí strany</param>
    /// <param name="serviceImplementationVersion">Verze proxy nad API třetí strany</param>
    /// <exception cref="CisConfigurationException">Chyba v konfiguraci služby - např. špatně zadaný typ implementace.</exception>
    /// <exception cref="CisConfigurationNotFound">Konfigurace pro klíč ES:{serviceName}:{serviceImplementationVersion} nebyla nalezena v sekci ExternalServices v appsettings.json</exception>
    public static TConfiguration AddExternalServiceConfigurationOfType<TClient, TConfiguration>(
        this WebApplicationBuilder builder,
        string serviceName,
        string serviceImplementationVersion)
        where TClient : class, IExternalServiceClient
        where TConfiguration : ExternalServiceConfiguration<TClient>
    {
        var configuration = (TConfiguration)Activator.CreateInstance(typeof(TConfiguration))!;

        builder.Configuration
            .GetSection(getSectionName(serviceName, serviceImplementationVersion))
            .Bind(configuration);

        validateConfiguration(configuration, serviceName, serviceImplementationVersion);
        addConfigurationToDi(builder.Services, (ExternalServiceConfiguration<TClient>)configuration, serviceName, serviceImplementationVersion);

        return configuration;
    }

    private static void addConfigurationToDi<TClient>(
        IServiceCollection services,
        ExternalServiceConfiguration<TClient> configuration,
        in string serviceName,
        in string serviceImplementationVersion)
        where TClient : class, IExternalServiceClient
    {
        configuration!.ServiceName = $"{Core.CisGlobalConstants.ExternalServicesServiceDiscoveryKeyPrefix}{serviceName}:{serviceImplementationVersion}";
        services.AddSingleton<IExternalServiceConfiguration<TClient>>(configuration);
    }

    /// <summary>
    /// Validace konfigurace
    /// </summary>
    private static void validateConfiguration<TClient>(
        IExternalServiceConfiguration<TClient> configuration, 
        in string serviceName,
        in string serviceImplementationVersion)
        where TClient : class, IExternalServiceClient
    {
        if (configuration == null)
            throw new CisConfigurationNotFound(getSectionName(serviceName, serviceImplementationVersion));
        if (!configuration.UseServiceDiscovery && configuration.ServiceUrl == null)
            throw new CisConfigurationException(0, $"{serviceName} Service URL must be defined");
        if (configuration.ImplementationType == Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");
    }

    private static string getSectionName(in string serviceName, in string serviceImplementationVersion)
        => $"{Core.CisGlobalConstants.ExternalServicesConfigurationSectionName}:{serviceName}:{serviceImplementationVersion}";
}
