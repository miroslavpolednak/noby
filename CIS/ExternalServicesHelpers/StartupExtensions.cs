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

        builder.Services.AddSingleton(provider =>
        {
            // pokud se ma hledat URL v service discovery
            if (configuration.UseServiceDiscovery)
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new($"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}:{serviceName}"), InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                configuration.ServiceUrl = url ?? throw new ArgumentNullException("url", $"Service Discovery can not find {Constants.ExternalServicesServiceDiscoveryKeyPrefix}:{serviceName} Proprietary service URL");
            }
            return configuration;
        });


        return configuration;
    }
}
