using CIS.Infrastructure.Messaging.KafkaFlow.Configuration;
using CIS.Infrastructure.Messaging.KafkaFlow.JsonSchema;
using CIS.Infrastructure.Messaging.KafkaFlow.Middlewares;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KafkaFlow;
using KafkaFlow.Configuration;
using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Messaging.KafkaFlow;

internal sealed class KafkaFlowMessagingConfigurator : IKafkaFlowMessagingConfigurator
{
    private readonly KafkaFlowConfiguratorSettings _settings;

    private Action<IClusterConfigurationBuilder> _clusterBuilder = delegate { };

    private KafkaFlowMessagingConfigurator(KafkaFlowConfiguratorSettings settings)
    {
        _settings = settings;
    }

    public static void Configure(KafkaFlowConfiguratorSettings settings, Action<IKafkaFlowMessagingConfigurator> messaging, IClusterConfigurationBuilder cluster)
    {
        var configurator = new KafkaFlowMessagingConfigurator(settings);

        messaging(configurator);

        configurator._clusterBuilder(cluster);
    }

    public IKafkaFlowMessagingConfigurator AddConsumerAvro<THandler>(string topic) where THandler : class, IMessageHandler =>
        AddConsumerAvro(topic, handlers => handlers.AddHandler<THandler>());

    public IKafkaFlowMessagingConfigurator AddConsumerAvro(string topic, Action<TypedHandlerConfigurationBuilder> handlers)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(topic, nameof(topic));

        _clusterBuilder += kafka => kafka.AddConsumer(consumer => SetupConsumer(consumer.Topic(topic), m => m.AddSchemaRegistryAvroDeserializer(), handlers));

        return this;
    }

    public IKafkaFlowMessagingConfigurator AddConsumerJson(string topic, Action<TypedHandlerConfigurationBuilder> handlers)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(topic, nameof(topic));

        _clusterBuilder += kafka => kafka.AddConsumer(consumer => SetupConsumer(consumer.Topic(topic), m => m.AddSchemaRegistryJsonDeserializer(), handlers));

        return this;
    }

    public IKafkaFlowMessagingConfigurator AddBatchConsumerAvro<TBatchHandler>(string topic) where TBatchHandler : class, IMessageMiddleware
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(topic, nameof(topic));

        _clusterBuilder += kafka => kafka.AddConsumer(consumer =>
        {
            consumer.Topic(topic).WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .WithGroupId(_settings.GroupId).WithBufferSize(300).WithWorkersCount(1)
                    .AddMiddlewares(middlewares =>
                    {
                        middlewares.AddSchemaRegistryAvroDeserializer();

                        middlewares.AddBatching(100, TimeSpan.FromSeconds(30));

                        middlewares.Add<ActivitySourceMiddleware>(MiddlewareLifetime.Message);
                        middlewares.Add<ConsumerErrorHandlingMiddleware>(MiddlewareLifetime.Singleton);

                        middlewares.Add<TBatchHandler>();
                    });
        });

        return this;
    }

    public IKafkaFlowMessagingConfigurator AddProducerAvro<TMessage>(string defaultTopic, SubjectNameStrategy subjectNameStrategy = SubjectNameStrategy.Record)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(defaultTopic, nameof(defaultTopic));

        _clusterBuilder += kafka => kafka.AddProducer<TMessage>(producer =>
        {
            producer.DefaultTopic(defaultTopic).WithAcks(Acks.All)
                    .AddMiddlewares(m =>
                    {
                        m.Add<DefaultKBAvroHeaderMiddleware>();

                        var avroSerializerConfig = new AvroSerializerConfig
                        {
                            SubjectNameStrategy = subjectNameStrategy,
                            AutoRegisterSchemas = false
                        };

                        m.AddSchemaRegistryAvroSerializer(avroSerializerConfig);
                    });
        });

        return this;
    }

    private void SetupConsumer(IConsumerConfigurationBuilder consumer, Action<IConsumerMiddlewareConfigurationBuilder> deserializerConfig, Action<TypedHandlerConfigurationBuilder> handlers)
    {
        consumer.WithAutoOffsetReset(AutoOffsetReset.Earliest)
                .WithGroupId(_settings.GroupId).WithManualMessageCompletion()
                .WithBufferSize(_settings.Configuration.BufferSize).WithWorkersCount(_settings.Configuration.WorkersCount)
                .AddMiddlewares(m =>
                {
                    m.AddAtBeginning<ActivitySourceMiddleware>(MiddlewareLifetime.Message);
                    m.Add<ConsumerErrorHandlingMiddleware>(MiddlewareLifetime.Singleton);

                    _settings.RetryStrategy.Configure(m); //Retry Middleware

                    deserializerConfig(m);

                    m.Add(resolver => new LoggingKnownMessagesMiddleware(
                              _settings.Configuration,
                              resolver.Resolve<ILogger<LoggingKnownMessagesMiddleware>>()
                          ),
                          MiddlewareLifetime.Singleton);

                    m.AddTypedHandlers(handlers += h => h.WithHandlerLifetime(InstanceLifetime.Scoped));
                });
    }
}