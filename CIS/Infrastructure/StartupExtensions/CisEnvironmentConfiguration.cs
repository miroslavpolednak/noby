using CIS.Core.Configuration;
using CIS.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisEnvironmentConfiguration
{
    public static ICisEnvironmentConfiguration AddCisEnvironmentConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.CisEnvironmentConfiguration cisConfiguration = new();
        builder.Configuration.GetSection(JsonConfigurationKey).Bind(cisConfiguration);

        CheckAndRegisterConfiguration(builder, cisConfiguration);

        // get env variables
        builder.Configuration.AddCisEnvironmentVariables($"{cisConfiguration.EnvironmentName}_");

        return cisConfiguration;
    }

    public static ICisEnvironmentConfiguration AddCisEnvironmentConfiguration(this WebApplicationBuilder builder, Action<ICisEnvironmentConfiguration> options)
    {
        var cisConfiguration = new Configuration.CisEnvironmentConfiguration();
        builder.Configuration.GetSection(JsonConfigurationKey).Bind(cisConfiguration);

        options.Invoke(cisConfiguration);

        CheckAndRegisterConfiguration(builder, cisConfiguration);

        // get env variables
        builder.Configuration.AddCisEnvironmentVariables($"{cisConfiguration.EnvironmentName}_");

        return cisConfiguration;
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

    private const string JsonConfigurationKey = "CisEnvironmentConfiguration";
}
