using CIS.Core.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisEnvironmentConfiguration
{
    public static WebApplicationBuilder AddCisEnvironmentConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.CisEnvironmentConfiguration cisConfiguration = new();
        builder.Configuration.GetSection(JsonConfigurationKey).Bind(cisConfiguration);

        CheckAndRegisterConfiguration(builder, cisConfiguration);

        // distributed cache
        registerDistributedCache(builder);

        return builder;
    }

    public static WebApplicationBuilder AddCisEnvironmentConfiguration(this WebApplicationBuilder builder, Action<ICisEnvironmentConfiguration> options)
    {
        var cisConfiguration = new Configuration.CisEnvironmentConfiguration();
        builder.Configuration.GetSection(JsonConfigurationKey).Bind(cisConfiguration);

        options.Invoke(cisConfiguration);

        CheckAndRegisterConfiguration(builder, cisConfiguration);

        // distributed cache
        registerDistributedCache(builder);

        return builder;
    }

    private static void CheckAndRegisterConfiguration(WebApplicationBuilder builder, ICisEnvironmentConfiguration cisConfiguration)
    {
        if (string.IsNullOrEmpty(cisConfiguration.DefaultApplicationKey))
            throw new ArgumentNullException("Application Key is empty, cannot initialize CIS Environment Configuration", "ApplicationKey");
        if (string.IsNullOrEmpty(cisConfiguration.EnvironmentName))
            throw new ArgumentNullException("Environment Name is empty, cannot initialize CIS Environment Configuration", "EnvironmentName");
        /*if (string.IsNullOrEmpty(cisConfiguration.ServiceDiscoveryUrl))
            throw new ArgumentNullException("Service Discovery Url is empty, cannot initialize CIS Environment Configuration", "ServiceDiscoveryUrl");*/

        builder.Services.TryAddSingleton(cisConfiguration);
    }

    private static void registerDistributedCache(WebApplicationBuilder builder)
    {
        var cacheConfiguration = new Configuration.CisEnvironmentDistributedCacheConfiguration();
        builder.Configuration.GetSection(JsonConfigurationKey).GetSection(JsonCacheConfigurationKey).Bind(cacheConfiguration);

        builder.Services.TryAddSingleton<ICisEnvironmentDistributedCacheConfiguration>(cacheConfiguration);
    }

    private const string JsonConfigurationKey = "CisEnvironmentConfiguration";
    private const string JsonCacheConfigurationKey = "DistributedCache";
}
