using CIS.Core.Exceptions.ExternalServices;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using System.Net;

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
        => builder.AddExternalServiceRestClient<TClient, TImplementation, IExternalServiceConfiguration<TClient>>();

    public static IHttpClientBuilder AddExternalServiceRestClient<TClient, TImplementation, TConfiguration>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
        where TImplementation : class, TClient
        where TConfiguration : IExternalServiceConfiguration
    {
        var client = builder
            .Services
            .AddHttpClient<TClient, TImplementation>((services, client) =>
            {
                var configuration = services.GetRequiredService<TConfiguration>();

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
                var configuration = services.GetRequiredService<TConfiguration>();

                // ignorovat vadny ssl certifikat
                var clientHandler = configuration.IgnoreServerCertificateErrors ? new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                } : new HttpClientHandler();

                // pouzij proxy
                if (configuration.UseDefaultProxy)
                {
                    clientHandler.Proxy = null;
                    clientHandler.UseProxy = true;
                    clientHandler.DefaultProxyCredentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                }

                // logovat payload a hlavicku
                if (configuration.UseLogging)
                {
                    var logger = services.GetRequiredService<ILogger<TClient>>();
                    return new HttpHandlers.LoggingHttpHandler(clientHandler, logger, configuration.LogRequestPayload, configuration.LogResponsePayload);
                }
                else
                    return clientHandler;
            });

            // set resiliency
            client.AddResilienceHandler($"{typeof(TClient)}", static (ResiliencePipelineBuilder<HttpResponseMessage> builder, ResilienceHandlerContext context) =>
            {
                var configuration = context.ServiceProvider.GetRequiredService<TConfiguration>();

                if (configuration.RequestRetryCount.GetValueOrDefault() > 0)
                {
                    builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                    {
                        BackoffType = DelayBackoffType.Constant,
                        UseJitter = false,
                        MaxRetryAttempts = configuration.RequestRetryCount!.Value,
                        Delay = TimeSpan.FromSeconds(getRequestRetryTimeout(configuration)),
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .HandleResult(response => (int)response.StatusCode >= 500 || response.StatusCode == HttpStatusCode.RequestTimeout)
                            .Handle<TimeoutRejectedException>()
                            .Handle<CisExtServiceUnavailableException>()
                            .Handle<CisExtServiceServerErrorException>()
                    });
                }

                builder.AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromSeconds(getRequestTimeout(configuration))
                });
            });

        return client;
    }
        

    private static int getRequestRetryTimeout(IExternalServiceConfiguration configuration)
        => configuration.RequestRetryTimeout ?? _defaultRetryTimeout;

    private static int getRequestTimeout(IExternalServiceConfiguration configuration)
        => configuration.RequestTimeout.GetValueOrDefault() > 0 ? configuration.RequestTimeout!.Value : _defaultRequestTimeout;

    private const int _defaultRequestTimeout = 10;
    private const int _defaultRetryTimeout = 10;
}
