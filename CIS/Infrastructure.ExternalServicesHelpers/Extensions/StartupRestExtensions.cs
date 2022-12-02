using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.AspNetCore.Builder;

namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class StartupRestExtensions
{
    /// <summary>
    /// Založení typed HttpClienta pro implementaci ExternalService.
    /// </summary>
    /// <typeparam name="TClient">Typ klienta - interface pro danou verzi proxy nad API třetí strany</typeparam>
    /// <typeparam name="TImplementation">Interní implementace TClient interface</typeparam>
    /// <typeparam name="TConfiguration">Typ konfigurace, který bude pro tohoto TClient vložen do Di</typeparam>
    /// <param name="builder"></param>
    /// <param name="serviceImplementationVersion">Verze proxy nad API třetí strany</param>
    /// <param name="additionalHandlersRegistration">Možnost zaregistrovat další HttpHandlery do pipeline.</param>
    public static IHttpClientBuilder AddExternalServiceRestClient<TClient, TImplementation, TConfiguration>(
        this WebApplicationBuilder builder,  
        string serviceImplementationVersion,
        IExternalServiceConfiguration configuration,
        Action<IHttpClientBuilder, IExternalServiceConfiguration>? additionalHandlersRegistration = null)
        where TClient : class, IExternalServiceClient
        where TImplementation : class, TClient
        where TConfiguration : class, IExternalServiceConfiguration<TClient>
    {
        var clientBuilder = builder.Services
            .AddHttpClient<TClient, TImplementation>((services, client) =>
            {
                var configurationInstance = services
                    .GetService<TConfiguration>()
                    ?? throw new CisConfigurationNotFound($"External service configuration of type {typeof(TConfiguration)} for {typeof(TClient)} version '{serviceImplementationVersion}' not found");

                // timeout requestu
                if (configurationInstance.RequestTimeout.GetValueOrDefault() > 0)
                    client.Timeout = TimeSpan.FromSeconds(configurationInstance.RequestTimeout!.Value);

                // service url
                client.BaseAddress = new Uri(configurationInstance.ServiceUrl);
            });

        // Prida do HttpClienta handler, ktery ignoruje vadne SSL certifikaty na cilovem serveru.
        if (configuration.IgnoreServerCertificateErrors)
        {
            clientBuilder.ConfigurePrimaryHttpMessageHandler(b =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                };
            });
        }

        // basic authentication
        if (configuration is IExternalServiceBasicAuthenticationConfiguration)
        {
            var basicAuthHeader = HttpHandlers.BasicAuthenticationHttpHandler.PrepareAuthorizationHeaderValue((IExternalServiceBasicAuthenticationConfiguration)configuration);
            builder.Services.AddSingleton(provider => new HttpHandlers.BasicAuthenticationHttpHandler(basicAuthHeader));
            clientBuilder.AddHttpMessageHandler<HttpHandlers.BasicAuthenticationHttpHandler>();
        }

        // zaregistrovat pripadne dalsi httpHandlery
        if (additionalHandlersRegistration != null)
            additionalHandlersRegistration(clientBuilder, configuration);

        // logovani payloadu
        if (configuration.LogPayloads)
        {
            builder.Services.AddSingleton<HttpHandlers.LoggingHttpHandler>();
            clientBuilder.AddHttpMessageHandler<HttpHandlers.LoggingHttpHandler>();
        }

        return clientBuilder;
    }    
}
