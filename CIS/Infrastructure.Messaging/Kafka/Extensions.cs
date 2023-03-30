using Avro.Specific;
using CIS.Infrastructure.Messaging.Kafka.Internals;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using KB.Speed.MassTransit.Kafka.Serializers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.Messaging.Kafka;

public static class Extensions
{
    public static void AddTopic<TMarker, TConsumer, TAvro>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        string topic,
        string groupId)
        where TMarker : class, ISpecificRecord
        where TConsumer : class, IConsumer
        where TAvro : class, ISpecificRecord, TMarker
        => addTopic<TMarker, TConsumer, TAvro, TConsumer, TAvro, TConsumer, TAvro>(factoryConfigurator, context, 1, topic, groupId);

    public static void AddTopic<TMarker, TConsumer1, TAvro1, TConsumer2, TAvro2>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        string topic,
        string groupId)
        where TMarker : class, ISpecificRecord
        where TConsumer1 : class, IConsumer
        where TAvro1 : class, ISpecificRecord, TMarker
        where TConsumer2 : class, IConsumer
        where TAvro2 : class, ISpecificRecord, TMarker
        => addTopic<TMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer1, TAvro1>(factoryConfigurator, context, 2, topic, groupId);

    public static void AddTopic<TMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer3, TAvro3>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        string topic,
        string groupId)
        where TMarker : class, ISpecificRecord
        where TConsumer1 : class, IConsumer
        where TAvro1 : class, ISpecificRecord, TMarker
        where TConsumer2 : class, IConsumer
        where TAvro2 : class, ISpecificRecord, TMarker
        where TConsumer3 : class, IConsumer
        where TAvro3 : class, ISpecificRecord, TMarker
        => addTopic<TMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer3, TAvro3>(factoryConfigurator, context, 3, topic, groupId);

    public static Configuration.IKafkaRiderConfiguration GetKafkaRiderConfiguration(this WebApplicationBuilder builder)
    {
        string configurationElement = $"{Constants.MessagingConfigurationElement}:{Constants.KafkaConfigurationElement}";

        var configuration = builder
            .Configuration
            .GetSection(configurationElement)
            .Get<Configuration.KafkaRiderConfiguration>()
            ?? throw new Core.Exceptions.CisConfigurationNotFound(configurationElement);

        // validovat konfiguraci
        configuration.ValidateConfiguration();

        return configuration;
    }

    public static void CreateKafkaHost(this IKafkaFactoryConfigurator factoryConfigurator, Configuration.IKafkaRiderConfiguration configuration)
    {
        factoryConfigurator.SecurityProtocol = configuration.SecurityProtocol;

        factoryConfigurator.Host(configuration.BootstrapServers, c =>
        {
            if (configuration.SecurityProtocol == SecurityProtocol.Ssl)
            {
                c.UseSsl(sslConfig =>
                {
                    sslConfig.EnableCertificateVerification = true;
                    sslConfig.SslCaLocation = configuration.SslCaLocation;
                    sslConfig.SslCertificateLocation = configuration.SslCertificateLocation;
                    sslConfig.KeyLocation = configuration.SslKeyLocation;
                    sslConfig.KeyPassword = configuration.SslKeyPassword;
                    sslConfig.SslCaCertificateStores = configuration.SslCaCertificateStores;
                });
            }
        });
    }

    private static void addTopic<TMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer3, TAvro3>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        int consumersCount,
        string topic,
        string groupId)
        where TMarker : class, ISpecificRecord
        where TConsumer1 : class, IConsumer
        where TAvro1 : class, ISpecificRecord, TMarker
        where TConsumer2 : class, IConsumer
        where TAvro2 : class, ISpecificRecord, TMarker
        where TConsumer3 : class, IConsumer
        where TAvro3 : class, ISpecificRecord, TMarker
    {
        var multipleTypeConfigBuilder = new MultipleTypeConfigBuilder<TMarker>();

        for (int i = 1; i <= consumersCount; i++)
        {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            _ = i switch
            {
                1 => addToBuilder<TAvro1>(),
                2 => addToBuilder<TAvro2>(),
                3 => addToBuilder<TAvro3>()
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
        }

        var multipleTypeConfig = multipleTypeConfigBuilder.Build();

        factoryConfigurator.TopicEndpoint<TMarker>(topic, groupId, conf =>
        {
            var schemaRegistryClient = context.GetRequiredService<ISchemaRegistryClient>();
            var valueDeserializer = new MultipleTypeDeserializer<TMarker>(multipleTypeConfig, schemaRegistryClient);

            conf.SetValueDeserializer(valueDeserializer.AsSyncOverAsync());
            conf.SetHeadersDeserializer(new HeaderDeserializer());

            for (int i = 1; i <= consumersCount; i++)
            {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
                _ = i switch
                {
                    1 => addToConfigurator<TConsumer1>(),
                    2 => addToConfigurator<TConsumer2>(),
                    3 => addToConfigurator<TConsumer3>()
                };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            }

            bool addToConfigurator<T>()
                where T : class, IConsumer
            {
                conf.ConfigureConsumer<T>(context);
                return true;
            }
        });

        bool addToBuilder<T>()
            where T : class, ISpecificRecord, TMarker
        {
            var avroInstance = Activator.CreateInstance<T>();
            multipleTypeConfigBuilder.AddType<T>(avroInstance.Schema);
            return true;
        }
    }
}
