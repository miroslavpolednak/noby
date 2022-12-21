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
    public static IHttpClientBuilder AddExternalServiceRestClient<TClient, TImplementation>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
        where TImplementation : class, TClient
        => builder.Services
            .AddHttpClient<TClient, TImplementation>((services, client) =>
            {
                var configuration = services.GetRequiredService<IExternalServiceConfiguration<TClient>>();

                // timeout requestu
                if (configuration.RequestTimeout.GetValueOrDefault() > 0)
                    client.Timeout = TimeSpan.FromSeconds(configuration.RequestTimeout!.Value);

                // service url
                client.BaseAddress = configuration.ServiceUrl;

                // authentication
                switch (configuration.Authentication)
                {
                    case ExternalServicesAuthenticationTypes.Basic:
                        var basicAuthHeader = HttpHandlers.BasicAuthenticationHttpHandler.PrepareAuthorizationHeaderValue(configuration);
                        client.DefaultRequestHeaders.Authorization = basicAuthHeader;
                        break;
                }
            })
            .ConfigurePrimaryHttpMessageHandler(services =>
            {
                var configuration = services.GetRequiredService<IExternalServiceConfiguration<TClient>>();
                
                // ignorovat vadny ssl certifikat
                var clientHandler = configuration.IgnoreServerCertificateErrors ? new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                    } : new HttpClientHandler();

                // logovat payload a hlavicku
                if (configuration.LogPayloads)
                {
                    var logger = services.GetRequiredService<ILogger<TClient>>();
                    return new HttpHandlers.LoggingHttpHandler(clientHandler, logger);
                }
                else
                    return clientHandler;
            });
}
