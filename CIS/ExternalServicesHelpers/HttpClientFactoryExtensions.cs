using CIS.ExternalServicesHelpers.Configuration;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.ExternalServicesHelpers;

public static class HttpClientFactoryExtensions
{
    public static IHttpClientBuilder AddExternalServiceClient<TClient, TImplementation, TConfiguration>(this WebApplicationBuilder builder, string serviceName, string serviceImplementationVersion)
        where TClient : class
        where TImplementation : class, TClient
        where TConfiguration : class, IExternalServiceConfiguration<TClient>
    {
        // ziskat konfiguraci sluzby z appsettings.json
        var configuration = builder.readConfiguration<TConfiguration>(serviceName, serviceImplementationVersion);
        builder.registerConfiguration(configuration, serviceName, serviceImplementationVersion);

        var clientBuilder = builder.Services.AddHttpClient<TClient, TImplementation>((services, client) =>
        {
            var configuration = services
                .GetService<TConfiguration>()
                ?? throw new CisConfigurationNotFound($"External service configuration of type {typeof(TConfiguration)} for {typeof(TClient)} version '{serviceImplementationVersion}' not found");

            // service url
            client.BaseAddress = new Uri(configuration.ServiceUrl);
        });

        return clientBuilder;
    }

    private static void registerConfiguration<TConfiguration>(this WebApplicationBuilder builder, TConfiguration configuration, string serviceName, string serviceImplementationVersion)
        where TConfiguration : class, IExternalServiceConfiguration
    {
        if (configuration.UseServiceDiscovery)
        {
            builder.Services.AddSingleton(provider =>
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceClient>()
                    .GetServiceUrlSynchronously(new($"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}{serviceName}"), InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                configuration.ServiceUrl = url ?? throw new ArgumentNullException("url", $"Service Discovery can not find {Constants.ExternalServicesConfigurationSectionName}:{serviceName}:{serviceImplementationVersion} Proprietary service URL");
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
        builder.Configuration.GetSection(getSectionName(serviceName)).Bind(configuration);

        if (configuration == null)
            throw new CisConfigurationNotFound(getSectionName(serviceName));
        if (!configuration.UseServiceDiscovery && string.IsNullOrEmpty(configuration.ServiceUrl))
            throw new CisConfigurationException(0, $"{serviceName} Service URL must be defined");
        if (configuration.ImplementationType == Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{serviceName} Service client Implementation type is not set");

        return configuration;
    }

    private static string getSectionName(string serviceName)
        => $"{Constants.ExternalServicesConfigurationSectionName}:{serviceName}";
}
