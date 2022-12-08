using CIS.InternalServices.ServiceDiscovery.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.ExternalServicesHelpers;

public static class StartupExtensions
{
    public static TConfiguration CreateAndCheckExternalServiceConfiguration<TConfiguration>(this WebApplicationBuilder builder, string serviceName)
        where TConfiguration : class, Configuration.IExternalServiceConfiguration
    {
        var configuration = readConfiguration<TConfiguration>(builder, serviceName);

        if (configuration.UseServiceDiscovery)
        {
            builder.Services.AddSingleton(provider =>
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceClient>()
                    .GetServiceUrlSynchronously(new($"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}{serviceName}"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                configuration.ServiceUrl = url ?? throw new ArgumentNullException("url", $"Service Discovery can not find {Constants.ExternalServicesConfigurationSectionName}:{serviceName} Proprietary service URL");
                return configuration;
            });
        }
        else
            builder.Services.AddSingleton(configuration);

        return configuration;
    }

    public static List<TConfiguration> CreateAndCheckExternalServiceConfigurationsList<TConfiguration>(this WebApplicationBuilder builder, string serviceName)
        where TConfiguration : class, Configuration.IExternalServiceConfiguration
    {
        var configurations = readConfigurations<TConfiguration>(builder, serviceName);

        builder.Services.AddSingleton(provider =>
        {
            foreach (var configuration in configurations)
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceClient>()
                    .GetServiceUrlSynchronously(new($"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}{serviceName}_{configuration.GetVersion()}"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                configuration.ServiceUrl = url ?? throw new ArgumentNullException("url", $"Service Discovery can not find {Constants.ExternalServicesConfigurationSectionName}:{serviceName}_{configuration.GetVersion()} Proprietary service URL");
            }

            return configurations;
        });

        return configurations;
    }

    private static List<TConfiguration> readConfigurations<TConfiguration>(WebApplicationBuilder builder, string serviceName)
        where TConfiguration : class, Configuration.IExternalServiceConfiguration
    {
        var listType = typeof(List<>);
        var constructedListType = listType.MakeGenericType(typeof(TConfiguration));
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        List<TConfiguration> configurations = (List<TConfiguration>)Activator.CreateInstance(constructedListType);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        builder.Configuration.GetSection(getSectionName(serviceName)).Bind(configurations);

        if (configurations == null)
            throw new CisConfigurationNotFound(getSectionName(serviceName));
        else if (!configurations.Any())
            throw new CisConfigurationNotFound(getSectionName(serviceName));

        configurations.ForEach(c =>
        {
            if (!c.UseServiceDiscovery && string.IsNullOrEmpty(c.ServiceUrl))
                throw new CisConfigurationException(0, $"{serviceName} Service URL must be defined");
            if (c.ImplementationType == Foms.Enums.ServiceImplementationTypes.Unknown)
                throw new CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");
        });

        return configurations;
    }

    private static TConfiguration readConfiguration<TConfiguration>(WebApplicationBuilder builder, string serviceName)
        where TConfiguration : class, Configuration.IExternalServiceConfiguration
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        TConfiguration configuration = (TConfiguration)Activator.CreateInstance(typeof(TConfiguration));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        builder.Configuration.GetSection(getSectionName(serviceName)).Bind(configuration);

        if (configuration == null)
            throw new CisConfigurationNotFound(getSectionName(serviceName));
        if (!configuration.UseServiceDiscovery && string.IsNullOrEmpty(configuration.ServiceUrl))
            throw new CisConfigurationException(0, $"{serviceName} Service URL must be defined");
        if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");

        return configuration;
    }

    private static string getSectionName(string serviceName)
        => $"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}";
}
