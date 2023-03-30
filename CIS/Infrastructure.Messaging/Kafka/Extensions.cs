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
        => addTopic<TMarker, TConsumer, TAvro, TConsumer, TAvro>(factoryConfigurator, context, 1, topic, groupId);

    internal static void addTopic<TMarker, TConsumer1, TAvro1, TConsumer2, TAvro2>(
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
    {
        
        // 2. configure multi type consumers
        var multipleTypeConfigBuilder = new MultipleTypeConfigBuilder<TMarker>();

        for (int i = 1; i <= consumersCount; i++)
        {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            _ = i switch
            {
                1 => addToBuilder<TAvro1>(multipleTypeConfigBuilder),
                2 => addToBuilder<TAvro1>(multipleTypeConfigBuilder),
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
                conf.ConfigureConsumer<TConsumer1>(context);
                conf.ConfigureConsumer<TConsumer2>(context);
            }
        });

        bool addToBuilder<T>(MultipleTypeConfigBuilder<TMarker> builder)
            where T : class, ISpecificRecord, TMarker
        {
            var avroInstance = Activator.CreateInstance<T>();
            builder.AddType<T>(avroInstance.Schema);
            return true;
        }
    }

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
}
