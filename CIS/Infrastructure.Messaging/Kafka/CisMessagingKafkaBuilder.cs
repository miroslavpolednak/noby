﻿using Avro.Specific;
using MassTransit;
using System.Reflection;

namespace CIS.Infrastructure.Messaging.Kafka;

internal sealed class CisMessagingKafkaBuilder : ICisMessagingKafkaBuilder
{
    public ICisMessagingKafkaBuilder AddConsumerTopicAvro<TTopicMarker>(string topic, string? groupId = null)
        where TTopicMarker : class, ISpecificRecord
    {
        _kafkaConfigurationActions.Add((configurator, context) =>
        {
            configurator.AddTopicEndpointAvro<TTopicMarker>(context, _consumerImplementations, topic, groupId, _contractsAssembly);
        });
        return this;
    }

    public ICisMessagingKafkaBuilder AddConsumerTopicJson<TTopicMarker>(string topic, string? groupId = null) where TTopicMarker : class
    {
        _kafkaConfigurationActions.Add((configurator, context) =>
        {
            configurator.AddTopicEndpointJson<TTopicMarker>(context, _consumerImplementations, topic, groupId, _contractsAssembly);
        });
        return this;
    }

    public ICisMessagingKafkaBuilder AddConsumer<TConsumer>()
        where TConsumer : class, IConsumer
    {
        _riderConfigurationActions.Add(configurator =>
        {
            configurator.AddConsumer<TConsumer>();
        });
        _consumerImplementations.Add(typeof(TConsumer));
        return this;
    }

    public ICisMessagingKafkaBuilder AddProducerAvro<TTopicMarker>(string topic)
        where TTopicMarker : class, ISpecificRecord
    {
        _riderConfigurationActions.Add(configurator =>
        {
            configurator.AddProducerAvro<TTopicMarker>(topic, _contractsAssembly);
        });
        return this;
    }

    public ICisMessagingKafkaBuilder AddProducerJson<TTopicMarker>(string topic)
        where TTopicMarker : class
    {
        _riderConfigurationActions.Add(configurator =>
        {
            configurator.AddProducerJson<TTopicMarker>(topic, _contractsAssembly);
        });
        return this;
    }
    
    public ICisMessagingKafkaBuilder AddRiderConfiguration(Action<IRiderRegistrationConfigurator> configuration)
    {
        _riderConfigurationActions.Add(configuration);
        return this;
    }

    public ICisMessagingKafkaBuilder AddRiderKafkaConfiguration(Action<IKafkaFactoryConfigurator, IRiderRegistrationContext> configuration)
    {
        _kafkaConfigurationActions.Add(configuration);
        return this;
    }

    public ICisMessagingBuilder Build()
    {
        if (_riderConfigurationActions.Count == 0)
        {
            throw new Core.Exceptions.CisConfigurationException(0, "Kafka Consumers and Producers collection is empty");
        }

        _builder.AppBuilder.Services.AddMassTransit(configurator =>
        {
            configurator.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });

            configurator.AddRider(rider =>
            {
                foreach (var action in _riderConfigurationActions)
                {
                    action(rider);
                }

                rider.UsingKafka((context, k) =>
                {
                    // kdyby vyhnila kafka, tak se zkousej porad rekonektit
                    k.ReconnectBackoff = TimeSpan.FromMilliseconds(250);
                    k.ReconnectBackoffMax = TimeSpan.FromMinutes(30);

                    k.CreateKafkaHost(_configuration);

                    k.UseSendFilter(typeof(Filters.KbHeadersSendFilter<>), context);

                    foreach (var action in _kafkaConfigurationActions)
                    {
                        action(k, context);
                    }
                });
            });
        });

        return _builder;
    }

    private readonly Configuration.IKafkaRiderConfiguration _configuration;
    private readonly CisMessagingBuilder _builder;
    private readonly Assembly _contractsAssembly;

    private List<Type> _consumerImplementations = new();
    private List<Action<IRiderRegistrationConfigurator>> _riderConfigurationActions = new();
    private List<Action<IKafkaFactoryConfigurator, IRiderRegistrationContext>> _kafkaConfigurationActions = new();

    public CisMessagingKafkaBuilder(CisMessagingBuilder builder, Configuration.IKafkaRiderConfiguration configuration, Assembly contractsAssembly)
    {
        _contractsAssembly = contractsAssembly;
        _builder = builder;
        _configuration = configuration;
    }
}
