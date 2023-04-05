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
using Microsoft.Extensions.Options;

namespace CIS.Infrastructure.Messaging.Kafka;

public static class KafkaExtensions
{
    internal static IRiderRegistrationConfigurator AddProducerAvro<TTopicMarker>(
        this IRiderRegistrationConfigurator rider,
        IEnumerable<Type> multipleTypes,
        string topic)
        where TTopicMarker : class, ISpecificRecord
    {
        var multipleTypeConfig = CreateMultipleTypeAvroConfig<TTopicMarker>(multipleTypes);

        rider.AddProducer<TTopicMarker>(topic, (riderContext, conf) =>
        {
            var schemaRegistryClient = riderContext.GetRequiredService<ISchemaRegistryClient>();
            var originalSerializerConfig = riderContext
                .GetRequiredService<IOptions<KB.Speed.Messaging.Kafka.SerDes.Avro.AvroSerializerConfig>>()
                .Value;
            var serializerConfig = new Confluent.SchemaRegistry.Serdes.AvroSerializerConfig
            {
                SubjectNameStrategy = originalSerializerConfig.SubjectNameStrategy,
                AutoRegisterSchemas = originalSerializerConfig.AutoRegisterSchemas
            };

            var valueSerializer = new MultipleTypeAvroSerializer<TTopicMarker>(multipleTypeConfig, schemaRegistryClient, serializerConfig);
            conf.SetValueSerializer(valueSerializer.AsSyncOverAsync());
        });

        return rider;
    }

    internal static IRiderRegistrationConfigurator AddProducerJson<TTopicMarker>(
        this IRiderRegistrationConfigurator rider,
        IEnumerable<Type> multipleTypes,
        string topic)
        where TTopicMarker : class
    {
        var multipleTypeConfig = CreateMultipleTypeJsonConfig<TTopicMarker>(multipleTypes);
        
        rider.AddProducer<TTopicMarker>(topic, (riderContext, conf) =>
        {
            var schemaRegistryClient = riderContext.GetRequiredService<ISchemaRegistryClient>();
            var serializerConfig = riderContext
                .GetRequiredService<IOptions<KB.Speed.Messaging.Kafka.SerDes.Json.JsonSerializerConfig>>()
                .Value;
            
            var valueSerializer = new MultipleTypeJsonSerializer<TTopicMarker>(multipleTypeConfig, schemaRegistryClient, serializerConfig);
            conf.SetValueSerializer(valueSerializer.AsSyncOverAsync());
        });

        return rider;
    }
    
    internal static IKafkaFactoryConfigurator AddTopicEndpoint<TTopicMarker>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        SchemaType schemaType,
        IRiderRegistrationContext context,
        IEnumerable<Type> multipleTypes,
        IEnumerable<Type> consumerImplementations,
        string topic,
        string? groupId)
        where TTopicMarker : class, ISpecificRecord
    {
        // get groupId
        var environmentConfiguration = context.GetRequiredService<CIS.Core.Configuration.ICisEnvironmentConfiguration>();

        factoryConfigurator.TopicEndpoint<TTopicMarker>(topic, groupId ?? environmentConfiguration.DefaultApplicationKey, conf =>
        {
            var schemaRegistryClient = context.GetRequiredService<ISchemaRegistryClient>();
            IAsyncDeserializer<TTopicMarker> valueDeserializer = schemaType switch
            {
                SchemaType.Avro => new MultipleTypeAvroDeserializer<TTopicMarker>(CreateMultipleTypeAvroConfig<TTopicMarker>(multipleTypes), schemaRegistryClient),
                SchemaType.Json => new MultipleTypeJsonDeserializer<TTopicMarker>(CreateMultipleTypeJsonConfig<TTopicMarker>(multipleTypes), schemaRegistryClient),
                _ => throw new NotSupportedException()
            };

            conf.SetValueDeserializer(valueDeserializer.AsSyncOverAsync());
            conf.SetHeadersDeserializer(new HeaderDeserializer());

            foreach (var impl in consumerImplementations)
            {
                conf.ConfigureConsumer(context, impl);
            }
        });

        return factoryConfigurator;
    }

    internal static MultipleTypeAvroConfig CreateMultipleTypeAvroConfig<TTopicMarker>(IEnumerable<Type> types)
        where TTopicMarker : class, ISpecificRecord
    {
        var multipleTypeConfigBuilder = new MultipleTypeAvroConfigBuilder<TTopicMarker>();

        foreach (var t in types)
        {
            var avroInstance = (ISpecificRecord)Activator.CreateInstance(t)!;
            multipleTypeConfigBuilder.AddType(t, avroInstance.Schema);
        }

        return multipleTypeConfigBuilder.Build();
    }

    internal static MultipleTypeJsonConfig CreateMultipleTypeJsonConfig<TTopicMarker>(IEnumerable<Type> types)
        where TTopicMarker : class
    {
        var multipleTypeConfigBuilder = new MultipleTypeJsonConfigBuilder<TTopicMarker>();

        foreach (var t in types)
        {
            multipleTypeConfigBuilder.AddType(t);
        }

        return multipleTypeConfigBuilder.Build();
    }    
    
    internal static Configuration.IKafkaRiderConfiguration GetKafkaRiderConfiguration(this WebApplicationBuilder builder)
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

    internal static void CreateKafkaHost(this IKafkaFactoryConfigurator factoryConfigurator, Configuration.IKafkaRiderConfiguration configuration)
    {
        if (!string.IsNullOrEmpty(configuration.Debug))
        {
            factoryConfigurator.Debug = configuration.Debug;
        }

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
