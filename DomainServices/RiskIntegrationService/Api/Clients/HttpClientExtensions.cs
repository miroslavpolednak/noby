﻿using CIS.ExternalServicesHelpers.Configuration;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http.Headers;

namespace DomainServices.RiskIntegrationService.Api.Clients;

internal static class HttpClientExtensions
{
    public static IHttpClientBuilder ConfigureC4mHttpMessageHandler(this IHttpClientBuilder builder)
        =>builder.ConfigurePrimaryHttpMessageHandler(()
            => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            });

    public static IHttpClientBuilder AddC4mPolicyHandler<TService>(this IHttpClientBuilder builder, string serviceName)
        => builder.AddPolicyHandler((services, request) => 
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1)
                    },
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        services.GetService<ILogger<TService>>()?.ExtServiceRetryCall(serviceName, retryAttempt, timespan.TotalMilliseconds);
                    }
                )
        );
    
    public static IHttpClientBuilder AddC4mHttpClient<TClient, TImplementation>(this IServiceCollection services, IExternalServiceBasicAuthenticationConfiguration configuration)
        where TClient : class
        where TImplementation : class, TClient
        => services.AddHttpClient<TClient, TImplementation>((services, client) =>
        {
            // service url
            client.BaseAddress = new Uri(configuration.ServiceUrl);

            // auth
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{configuration.Username}:{configuration.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
        });
}
