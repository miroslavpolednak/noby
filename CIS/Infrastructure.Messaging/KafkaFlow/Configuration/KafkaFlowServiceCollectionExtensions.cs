using CIS.Infrastructure.Messaging.KafkaFlow.Configuration.SchemaRegistry;
using Confluent.SchemaRegistry;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration;

internal static class KafkaFlowServiceCollectionExtensions
{
    public static IServiceCollection AddSchemaRegistryClient(this IServiceCollection services, KafkaFlowConfiguratorSettings settings)
    {
        var httpClientRetryPolicy = HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        services.AddSingleton(settings.Configuration.SchemaRegistry);
        services.AddHttpClient<ISchemaRegistryClient, ApicurioSchemaRegistryClient>()
                .AddPolicyHandler(httpClientRetryPolicy);

        return services;
    }
}