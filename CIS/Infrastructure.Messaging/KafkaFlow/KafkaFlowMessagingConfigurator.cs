﻿using CIS.Infrastructure.Messaging.KafkaFlow.Configuration;
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

        SetAdminMessages();
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

        _clusterBuilder += kafka => kafka.AddConsumer(consumer =>
        {
            consumer.Topic(topic).WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .WithGroupId(_settings.GroupId).WithManualMessageCompletion()
                    .WithBufferSize(2).WithWorkersCount(10)
                    .AddMiddlewares(m =>
                    {
                        m.AddAtBeginning<ActivitySourceMiddleware>(MiddlewareLifetime.Message);
                        m.Add<ConsumerErrorHandlingMiddleware>(MiddlewareLifetime.Singleton);

                        _settings.RetryStrategy.Configure(m); //Retry Middleware

                        m.AddSchemaRegistryAvroDeserializer();

                        m.Add(resolver => new LoggingKnownMessagesMiddleware(
                                  _settings.Configuration,
                                  resolver.Resolve<ILogger<LoggingKnownMessagesMiddleware>>()
                              ), MiddlewareLifetime.Singleton);

                        m.AddTypedHandlers(handlers += h => h.WithHandlerLifetime(InstanceLifetime.Scoped));
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

    private void SetAdminMessages()
    {
        if (string.IsNullOrWhiteSpace(_settings.Configuration.AdminTopic))
            return;

        _clusterBuilder += kafka => kafka.EnableTelemetry(_settings.Configuration.AdminTopic).EnableAdminMessages(_settings.Configuration.AdminTopic);
    }
}