using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using CIS.ExternalServicesHelpers;
using Polly;
using Polly.Extensions.Http;
using Microsoft.Net.Http.Headers;
using CIS.InternalServices.ServiceDiscovery.Abstraction;

namespace ExternalServices.SbWebApi;

public static class StartupExtensions
{
    internal const string ServiceName = "SbWebApi";

    public static WebApplicationBuilder AddExternalServiceSbWebApi(this WebApplicationBuilder builder)
    {
        var configuration = builder.CreateAndCheckExternalServiceConfiguration<Configuration.SbWebApiConfiguration>(ServiceName);

        switch (configuration.Version)
        {
            case Versions.V1:
                if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    builder.Services.AddScoped<V1.ISbWebApiClient, V1.MockSbWebApiClient>();
                else
                    builder.Services.AddHttpClient<V1.ISbWebApiClient, V1.Clients.RealSbWebApiClient>((services, client) =>
                    {
                        client.Timeout = TimeSpan.FromSeconds(configuration.RequestTimeout ?? 10);
                        client.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.ContentType, "application/json");

                        // service url
                        if (configuration.UseServiceDiscovery)
                        {
                            string url = services.GetRequiredService<IDiscoveryServiceAbstraction>()
                                .GetServiceUrlSynchronously(new($"{Constants.ExternalServicesServiceDiscoveryKeyPrefix}{ServiceName}"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary);
                            client.BaseAddress = new Uri(url!);
                        }
                        else
                            client.BaseAddress = new Uri(configuration.ServiceUrl);
                    })
                        .AddPolicyHandler((services, request) => HttpPolicyExtensions
                            .HandleTransientHttpError()
                            .WaitAndRetryAsync(new[]
                            {
                                TimeSpan.FromSeconds(1),
                                TimeSpan.FromSeconds(2)
                            },
                            onRetry: (outcome, timespan, retryAttempt, context) =>
                            {
                                services.GetService<ILogger<V1.ISbWebApiClient>>()?.ExtServiceRetryCall(ServiceName, retryAttempt, timespan.TotalMilliseconds);
                            }
                            ));
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {configuration.Version} client not implemented");
        }

        return builder;
    }

    
}
