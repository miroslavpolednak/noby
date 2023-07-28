using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Google.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System.Configuration;
using System.Net.Sockets;

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

                // service url
                client.BaseAddress = configuration.ServiceUrl;
                // musi byt nastaveny, jinak je default na 100
                client.Timeout = TimeSpan.FromSeconds(
                    (getRequestTimeout(configuration) * (configuration.RequestRetryCount.GetValueOrDefault() + 1)) 
                    + (configuration.RequestRetryCount.GetValueOrDefault() * getRequestRetryTimeout(configuration)) 
                    + 10);

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
                if (configuration.UseLogging)
                {
                    var logger = services.GetRequiredService<ILogger<TClient>>();
                    return new HttpHandlers.LoggingHttpHandler(clientHandler, logger, configuration.LogRequestPayload, configuration.LogResponsePayload);
                }
                else
                    return clientHandler;
            })
            // set retry policy
            .AddPolicyHandler((services, req) =>
            {
                var configuration = services.GetRequiredService<IExternalServiceConfiguration<TClient>>();
                var logger = services.GetRequiredService<ILogger<TClient>>();

                if (configuration.RequestRetryCount.GetValueOrDefault() == 0)
                {
                    return Policy.NoOpAsync<HttpResponseMessage>();
                }
                else
                {
                    return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .Or<TimeoutRejectedException>()
                        .WaitAndRetryAsync(configuration.RequestRetryCount!.Value, (c) => TimeSpan.FromSeconds(getRequestRetryTimeout(configuration)), (res, timeSpan, count, context) =>
                        {
                            logger.HttpRequestRetry(typeof(TClient).Name, count);
                        });
                }
            })
            // set timeout requestu
            .AddPolicyHandler((services, req) =>
            {
                var configuration = services.GetRequiredService<IExternalServiceConfiguration<TClient>>();
                return Policy.TimeoutAsync<HttpResponseMessage>(getRequestTimeout(configuration));
            });

    private static int getRequestRetryTimeout<TClient>(IExternalServiceConfiguration<TClient> configuration)
        where TClient : class, IExternalServiceClient
        => configuration.RequestRetryTimeout ?? _defaultRetryTimeout;

    private static int getRequestTimeout<TClient>(IExternalServiceConfiguration<TClient> configuration)
        where TClient : class, IExternalServiceClient
        => configuration.RequestTimeout.GetValueOrDefault() > 0 ? configuration.RequestTimeout!.Value : _defaultRequestTimeout;

    private const int _defaultRequestTimeout = 10;
    private const int _defaultRetryTimeout = 10;
}
