using CIS.Infrastructure.Messaging.KafkaFlow.Configuration.SchemaRegistry;
using Confluent.SchemaRegistry;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration;

internal static class KafkaFlowExtensions
{
    public static IServiceCollection AddSchemaRegistryClient(this IServiceCollection services, KafkaFlowConfiguratorSettings settings)
    {
        if (settings.Configuration.SchemaRegistry is null)
            return services;

        var httpClientRetryPolicy = HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        services.AddSingleton(settings.Configuration.SchemaRegistry);
        services.AddHttpClient<ISchemaRegistryClient, ApicurioSchemaRegistryClient>()
                .AddPolicyHandler(httpClientRetryPolicy);

        return services;
    }

    public static IClusterConfigurationBuilder EnableCisTelemetry(this IClusterConfigurationBuilder cluster, string topicName, int topicPartition = 0)
    {
        cluster.DependencyConfigurator.AddSingleton<MultiBrokerTelemetryScheduler>();

        var telemetryId = $"telemetry-{Convert.ToBase64String(Guid.NewGuid().ToByteArray())}";

        return cluster.AddProducer(
                          telemetryId,
                          producer => producer
                                      .DefaultTopic(topicName)
                                      .AddMiddlewares(
                                          middlewares => middlewares
                                              .AddSerializer<ProtobufNetSerializer>()))
                      .OnStarted(resolver => resolver.Resolve<MultiBrokerTelemetryScheduler>().Start(telemetryId, topicName, topicPartition))
                      .OnStopping(resolver => resolver.Resolve<MultiBrokerTelemetryScheduler>().Stop(telemetryId));
    }
}