using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.AspNetCore.Builder;

namespace CIS.Infrastructure.ExternalServicesHelpers;

public static class StartupRestExtensions
{
    /// <summary>
    /// Založení typed HttpClienta pro implementaci ExternalService.
    /// </summary>
    /// <remarks>
    /// Některé HttpHandlery jsou vkládané pomocí konfigurace - to je proto, že potřebujeme na úrovni CI/CD řešit, zda budou v pipeline nebo ne.
    /// </remarks>
    /// <typeparam name="TClient">Typ klienta - interface pro danou verzi proxy nad API třetí strany</typeparam>
    /// <typeparam name="TImplementation">Interní implementace TClient interface</typeparam>
    /// <param name="builder"></param>
    /// <param name="additionalHandlersRegistration">Možnost zaregistrovat další HttpHandlery do pipeline.</param>
    public static IHttpClientBuilder AddExternalServiceRestClient<TClient, TImplementation>(
        this WebApplicationBuilder builder,  
        IExternalServiceConfiguration configuration,
        Action<IHttpClientBuilder, IExternalServiceConfiguration>? additionalHandlersRegistration = null)
        where TClient : class, IExternalServiceClient
        where TImplementation : class, TClient
    {
        var clientBuilder = builder.Services
            .AddHttpClient<TClient, TImplementation>((services, client) =>
            {
                var configurationInstance = services
                    .GetService<IExternalServiceConfiguration<TClient>>()
                    ?? throw new CisConfigurationNotFound($"{typeof(TClient)}");

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

        // authentication
        switch (configuration.Authentication)
        {
            case ExternalServicesAuthenticationTypes.Basic:
                var basicAuthHeader = HttpHandlers.BasicAuthenticationHttpHandler.PrepareAuthorizationHeaderValue(configuration);
                builder.Services.AddSingleton(provider => new HttpHandlers.BasicAuthenticationHttpHandler(basicAuthHeader));
                clientBuilder.AddHttpMessageHandler<HttpHandlers.BasicAuthenticationHttpHandler>();
                break;
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
