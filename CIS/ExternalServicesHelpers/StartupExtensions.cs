using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.ExternalServicesHelpers;

public static class StartupExtensions
{
    public static TConfiguration CreateAndCheckExternalServiceConfiguration<TConfiguration>(this WebApplicationBuilder builder, string serviceName)
        where TConfiguration : class, Configuration.IExternalServiceConfiguration
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        TConfiguration configuration = (TConfiguration)Activator.CreateInstance(typeof(TConfiguration));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        string sectionName = $"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}";
        builder.Configuration.GetSection(sectionName).Bind(configuration);

        if (configuration == null)
            throw new Core.Exceptions.CisConfigurationNotFound(sectionName);
        if (!configuration.UseServiceDiscovery && string.IsNullOrEmpty(configuration.ServiceUrl))
            throw new Core.Exceptions.CisConfigurationException(0, $"{serviceName} Service URL must be defined");
        if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new Core.Exceptions.CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");

        builder.Services.AddSingleton(configuration);

        return configuration;
    }

    public static TConfiguration CreateAndCheckExternalServiceConfigurationWithServiceDiscovery<TConfiguration>(this WebApplicationBuilder builder, string serviceName)
        where TConfiguration : class, Configuration.IExternalServiceConfiguration
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        TConfiguration configuration = (TConfiguration)Activator.CreateInstance(typeof(TConfiguration));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        string sectionName = $"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}";
        builder.Configuration.GetSection(sectionName).Bind(configuration);

        if (configuration == null)
            throw new Core.Exceptions.CisConfigurationNotFound(sectionName);
        if (!configuration.UseServiceDiscovery && string.IsNullOrEmpty(configuration.ServiceUrl))
            throw new Core.Exceptions.CisConfigurationException(0, $"{serviceName} Service URL must be defined");
        if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new Core.Exceptions.CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");

        if (configuration.UseServiceDiscovery)
        {
            builder.Services.AddSingleton(provider =>
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetServiceUrlSynchronously(new($"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
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
        var listType = typeof(List<>);
        var constructedListType = listType.MakeGenericType(typeof(TConfiguration));
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        List<TConfiguration> configurations = (List<TConfiguration>)Activator.CreateInstance(constructedListType);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        string sectionName = $"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}";
        builder.Configuration.GetSection(sectionName).Bind(configurations);

        if (configurations == null)
            throw new Core.Exceptions.CisConfigurationNotFound(sectionName);
        else if (!configurations.Any())
            throw new Core.Exceptions.CisConfigurationNotFound(sectionName);

        configurations.ForEach(c =>
        {
            if (!c.UseServiceDiscovery && string.IsNullOrEmpty(c.ServiceUrl))
                throw new Core.Exceptions.CisConfigurationException(0, $"{serviceName} Service URL must be defined");
            if (c.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Unknown)
                throw new Core.Exceptions.CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");
        });
        
        builder.Services.AddSingleton(configurations);

        return configurations;
    }
}
