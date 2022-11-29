﻿using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class StartupExtensions
{
    /// <summary>
    /// Založení typed HttpClienta
    /// </summary>
    /// <typeparam name="TClient">Typ klienta - interface pro danou verzi proxy nad API třetí strany</typeparam>
    /// <typeparam name="TImplementation">Interní implementace TClient interface</typeparam>
    /// <typeparam name="TConfiguration">Typ konfigurace, který bude pro tohoto TClient vložen do Di</typeparam>
    /// <param name="builder"></param>
    /// <param name="serviceName">Název konzumované služby třetí strany</param>
    /// <param name="serviceImplementationVersion">Verze proxy nad API třetí strany</param>
    /// <exception cref="CisConfigurationException">Chyba v konfiguraci služby - např. špatně zadaný typ implementace.</exception>
    /// <exception cref="CisConfigurationNotFound">Konfigurace typu TConfiguration pro klíč ES:{serviceName}:{serviceImplementationVersion} nebyla nalezena v sekci ExternalServices v appsettings.json</exception>
    public static IHttpClientBuilder AddExternalServiceClient<TClient, TImplementation, TConfiguration>(this WebApplicationBuilder builder, string serviceName, string serviceImplementationVersion)
        where TClient : class
        where TImplementation : class, TClient
        where TConfiguration : class, IExternalServiceConfiguration<TClient>
    {
        // ziskat konfiguraci sluzby z appsettings.json
        var configuration = builder.readConfiguration<TConfiguration>(serviceName, serviceImplementationVersion);
        builder.registerConfiguration(configuration, serviceName, serviceImplementationVersion);

        var clientBuilder = builder.Services
            .AddHttpClient<TClient, TImplementation>((services, client) =>
            {
                var configuration = services
                    .GetService<TConfiguration>()
                    ?? throw new CisConfigurationNotFound($"External service configuration of type {typeof(TConfiguration)} for {typeof(TClient)} version '{serviceImplementationVersion}' not found");

                // service url
                client.BaseAddress = new Uri(configuration.ServiceUrl);
            });

        // poslat v requestu hlavicky vyzadovane v KB
        if (configuration.PropagateKbHeaders)
        {
            builder.Services.AddTransient<HttpHandlers.KbHeadersHttpHandler>(provider => new HttpHandlers.KbHeadersHttpHandler(configuration));
            clientBuilder.AddHttpMessageHandler<HttpHandlers.KbHeadersHttpHandler>();
        }

        // basic authentication
        if (configuration is IExternalServiceBasicAuthenticationConfiguration)
        {
            var basicAuthHeader = HttpHandlers.BasicAuthenticationHttpHandler.PrepareAuthorizationHeaderValue((IExternalServiceBasicAuthenticationConfiguration)configuration);
            builder.Services.AddTransient<HttpHandlers.BasicAuthenticationHttpHandler>(provider => new HttpHandlers.BasicAuthenticationHttpHandler(basicAuthHeader));
            clientBuilder.AddHttpMessageHandler<HttpHandlers.BasicAuthenticationHttpHandler>();
        }

        // logovani payloadu
        if (configuration.LogPayloads)
        {
            builder.Services.AddTransient<HttpHandlers.LoggingHttpHandler>();
            clientBuilder.AddHttpMessageHandler<HttpHandlers.LoggingHttpHandler>();
        }

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
                    .GetRequiredService<IDiscoveryServiceAbstraction>()
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
