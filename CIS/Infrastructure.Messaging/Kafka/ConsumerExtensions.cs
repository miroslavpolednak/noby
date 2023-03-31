﻿using Avro.Specific;
using CIS.Infrastructure.Messaging.Kafka.Internals;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using KB.Speed.MassTransit.Kafka.Serializers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.Messaging.Kafka;

public static class ConsumerExtensions
{
    public static IKafkaFactoryConfigurator AddTopic<TTopicMarker, TConsumer, TAvro>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        string topic,
        string? groupId = null)
        where TTopicMarker : class, ISpecificRecord
        where TConsumer : class, IConsumer
        where TAvro : class, ISpecificRecord, TTopicMarker
        => addTopic<TTopicMarker, TConsumer, TAvro, TConsumer, TAvro, TConsumer, TAvro, TConsumer, TAvro>(factoryConfigurator, context, 1, topic, groupId);

    public static IKafkaFactoryConfigurator AddTopic<TTopicMarker, TConsumer1, TAvro1, TConsumer2, TAvro2>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        string topic,
        string? groupId = null)
        where TTopicMarker : class, ISpecificRecord
        where TConsumer1 : class, IConsumer
        where TAvro1 : class, ISpecificRecord, TTopicMarker
        where TConsumer2 : class, IConsumer
        where TAvro2 : class, ISpecificRecord, TTopicMarker
        => addTopic<TTopicMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer1, TAvro1, TConsumer1, TAvro1>(factoryConfigurator, context, 2, topic, groupId);

    public static IKafkaFactoryConfigurator AddTopic<TTopicMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer3, TAvro3>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        string topic,
        string? groupId = null)
        where TTopicMarker : class, ISpecificRecord
        where TConsumer1 : class, IConsumer
        where TAvro1 : class, ISpecificRecord, TTopicMarker
        where TConsumer2 : class, IConsumer
        where TAvro2 : class, ISpecificRecord, TTopicMarker
        where TConsumer3 : class, IConsumer
        where TAvro3 : class, ISpecificRecord, TTopicMarker
        => addTopic<TTopicMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer3, TAvro3, TConsumer1, TAvro1>(factoryConfigurator, context, 3, topic, groupId);

    public static IKafkaFactoryConfigurator AddTopic<TTopicMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer3, TAvro3, TConsumer4, TAvro4>(
        this IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        string topic,
        string? groupId = null)
        where TTopicMarker : class, ISpecificRecord
        where TConsumer1 : class, IConsumer
        where TAvro1 : class, ISpecificRecord, TTopicMarker
        where TConsumer2 : class, IConsumer
        where TAvro2 : class, ISpecificRecord, TTopicMarker
        where TConsumer3 : class, IConsumer
        where TAvro3 : class, ISpecificRecord, TTopicMarker
        where TConsumer4 : class, IConsumer
        where TAvro4 : class, ISpecificRecord, TTopicMarker
        => addTopic<TTopicMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer3, TAvro3, TConsumer4, TAvro4>(factoryConfigurator, context, 4, topic, groupId);

    private static IKafkaFactoryConfigurator addTopic<TTopicMarker, TConsumer1, TAvro1, TConsumer2, TAvro2, TConsumer3, TAvro3, TConsumer4, TAvro4>(
        IKafkaFactoryConfigurator factoryConfigurator,
        IRiderRegistrationContext context,
        int consumersCount,
        string topic,
        string? groupId)
        where TTopicMarker : class, ISpecificRecord
        where TConsumer1 : class, IConsumer
        where TAvro1 : class, ISpecificRecord, TTopicMarker
        where TConsumer2 : class, IConsumer
        where TAvro2 : class, ISpecificRecord, TTopicMarker
        where TConsumer3 : class, IConsumer
        where TAvro3 : class, ISpecificRecord, TTopicMarker
        where TConsumer4 : class, IConsumer
        where TAvro4 : class, ISpecificRecord, TTopicMarker
    {
        var multipleTypeConfigBuilder = new MultipleTypeConfigBuilder<TTopicMarker>();

        for (int i = 1; i <= consumersCount; i++)
        {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            _ = i switch
            {
                1 => addToBuilder<TAvro1>(),
                2 => addToBuilder<TAvro2>(),
                3 => addToBuilder<TAvro3>(),
                4 => addToBuilder<TAvro4>()
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
        }

        var multipleTypeConfig = multipleTypeConfigBuilder.Build();

        // get groupId
        var environmentConfiguration = context.GetRequiredService<CIS.Core.Configuration.ICisEnvironmentConfiguration>();
        
        factoryConfigurator.TopicEndpoint<TTopicMarker>(topic, groupId ?? environmentConfiguration.EnvironmentName, conf =>
        {
            var schemaRegistryClient = context.GetRequiredService<ISchemaRegistryClient>();
            var valueDeserializer = new MultipleTypeDeserializer<TTopicMarker>(multipleTypeConfig, schemaRegistryClient);

            conf.SetValueDeserializer(valueDeserializer.AsSyncOverAsync());
            conf.SetHeadersDeserializer(new HeaderDeserializer());

            for (int i = 1; i <= consumersCount; i++)
            {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
                _ = i switch
                {
                    1 => addToConfigurator<TConsumer1>(),
                    2 => addToConfigurator<TConsumer2>(),
                    3 => addToConfigurator<TConsumer3>(),
                    4 => addToConfigurator<TConsumer4>()
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

        return factoryConfigurator;

        bool addToBuilder<T>()
            where T : class, ISpecificRecord, TTopicMarker
        {
            var avroInstance = Activator.CreateInstance<T>();
            multipleTypeConfigBuilder.AddType<T>(avroInstance.Schema);
            return true;
        }
    }
}
