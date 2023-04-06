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
        string topic)
        where TTopicMarker : class, ISpecificRecord
    {
        var multipleTypeConfig = CreateMultipleTypeAvroConfig<TTopicMarker>();

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
        string topic)
        where TTopicMarker : class
    {
        var multipleTypeConfig = CreateMultipleTypeJsonConfig<TTopicMarker>();
        
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
    
    internal static IKafkaFactoryConfigurator AddTopicEndpointAvro<TTopicMarker>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
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
            var multipleTypeConfig = CreateMultipleTypeAvroConfig<TTopicMarker>();
            var valueDeserializer = new MultipleTypeAvroDeserializer<TTopicMarker>(multipleTypeConfig, schemaRegistryClient);

            conf.SetValueDeserializer(valueDeserializer.AsSyncOverAsync());
            conf.SetHeadersDeserializer(new HeaderDeserializer());

            foreach (var impl in consumerImplementations)
            {
                conf.ConfigureConsumer(context, impl);
            }
        });

        return factoryConfigurator;
    }

    internal static IKafkaFactoryConfigurator AddTopicEndpointJson<TTopicMarker>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        IEnumerable<Type> consumerImplementations,
        string topic,
        string? groupId)
        where TTopicMarker : class
    {
        // get groupId
        var environmentConfiguration = context.GetRequiredService<CIS.Core.Configuration.ICisEnvironmentConfiguration>();

        factoryConfigurator.TopicEndpoint<TTopicMarker>(topic, groupId ?? environmentConfiguration.DefaultApplicationKey, conf =>
        {
            var schemaRegistryClient = context.GetRequiredService<ISchemaRegistryClient>();
            var multipleTypeConfig = CreateMultipleTypeJsonConfig<TTopicMarker>();
            var valueDeserializer = new MultipleTypeJsonDeserializer<TTopicMarker>(multipleTypeConfig, schemaRegistryClient);

            conf.SetValueDeserializer(valueDeserializer.AsSyncOverAsync());
            conf.SetHeadersDeserializer(new HeaderDeserializer());

            foreach (var impl in consumerImplementations)
            {
                conf.ConfigureConsumer(context, impl);
            }
        });

        return factoryConfigurator;
    }
    
    
    internal static IEnumerable<Type> GetContractTypes<TTopicMarker>()
    {
        var types = System.Reflection.Assembly.GetEntryAssembly()!
            .GetTypes()
            .Where(type => typeof(TTopicMarker).IsAssignableFrom(type) && !type.IsInterface)
            .ToList();
        
        if (!types.Any())
        {
            throw new Core.Exceptions.CisArgumentException(0, $"No contracts implementing {typeof(TTopicMarker)} found.");
        }

        return types;
    }
    
    internal static MultipleTypeAvroConfig CreateMultipleTypeAvroConfig<TTopicMarker>()
        where TTopicMarker : class, ISpecificRecord
    {
        var types = GetContractTypes<TTopicMarker>();
        var multipleTypeConfigBuilder = new MultipleTypeAvroConfigBuilder<TTopicMarker>();

        foreach (var t in types)
        {
            var avroInstance = (ISpecificRecord)Activator.CreateInstance(t)!;
            multipleTypeConfigBuilder.AddType(t, avroInstance.Schema);
        }

        return multipleTypeConfigBuilder.Build();
    }

    internal static MultipleTypeJsonConfig CreateMultipleTypeJsonConfig<TTopicMarker>()
        where TTopicMarker : class
    {
        var types = GetContractTypes<TTopicMarker>();
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
