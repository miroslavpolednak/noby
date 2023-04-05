using Avro.Specific;
using Confluent.SchemaRegistry;
using MassTransit;

namespace CIS.Infrastructure.Messaging.Kafka;

internal sealed class CisMessagingKafkaBuilder : ICisMessagingKafkaBuilder
{
    public ICisMessagingKafkaBuilder AddConsumerTopicAvro<TTopicMarker>(string topic, string? groupId = null)
        where TTopicMarker : class, ISpecificRecord
    {
        var multipleTypes = getTypes<TTopicMarker>();
        if (!multipleTypes.Any())
        {
            throw new Core.Exceptions.CisArgumentException(0, $"No consumer contracts implementing {typeof(TTopicMarker)} found", "consumers");
        }

        _kafkaConfigurationActions.Add((configurator, context) =>
        {
            configurator.AddTopicEndpoint<TTopicMarker>(SchemaType.Avro, context, multipleTypes, _consumerImplementations, topic, groupId);
        });
        return this;
    }

    public ICisMessagingKafkaBuilder AddConsumerTopicJson<TTopicMarker>(string topic, string? groupId = null) where TTopicMarker : class
    {
        // TODO: implement
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
        var multipleTypes = getTypes<TTopicMarker>();
        if (!multipleTypes.Any())
        {
            throw new Core.Exceptions.CisArgumentException(0, $"No producer contracts implementing {typeof(TTopicMarker)} found", "producers");
        }

        _riderConfigurationActions.Add(configurator =>
        {
            configurator.AddProducerAvro<TTopicMarker>(multipleTypes, topic);
        });

        return this;
    }

    public ICisMessagingKafkaBuilder AddProducerJson<TTopicMarker>(string topic) where TTopicMarker : class
    {
        // TODO: implement
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

    private static IEnumerable<Type> getTypes<T>()
    {
        return System.Reflection.Assembly.GetEntryAssembly()!
            .GetTypes()
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface)
            .ToList();
    }

    private readonly Configuration.IKafkaRiderConfiguration _configuration;
    private readonly CisMessagingBuilder _builder;

    private List<Type> _consumerImplementations = new();
    private List<Action<IRiderRegistrationConfigurator>> _riderConfigurationActions = new();
    private List<Action<IKafkaFactoryConfigurator, IRiderRegistrationContext>> _kafkaConfigurationActions = new();

    public CisMessagingKafkaBuilder(CisMessagingBuilder builder, Configuration.IKafkaRiderConfiguration configuration)
    {
        _builder = builder;
        _configuration = configuration;
    }
}
