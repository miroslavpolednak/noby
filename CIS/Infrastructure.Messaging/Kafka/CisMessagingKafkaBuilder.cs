using MassTransit;

namespace CIS.Infrastructure.Messaging.Kafka;

internal sealed class CisMessagingKafkaBuilder
    : ICisMessagingKafkaBuilder
{
    public ICisMessagingKafkaBuilder AddConsumers(Action<IRiderRegistrationConfigurator> action)
    {
        _riderActions.Add(action);
        return this;
    }

    public ICisMessagingKafkaBuilder AddConsumersToTopic(Action<IKafkaFactoryConfigurator, IRiderRegistrationContext> action)
    {
        _configurationActions.Add(action);
        return this;
    }

    public ICisMessagingBuilder Build()
    {
        //TODO dodelat producer check
        if (_riderActions.Count == 0)
        {
            throw new Core.Exceptions.CisConfigurationException(0, "Kafka Consumers collection is empty");
        }

        _builder.AppBuilder.Services.AddMassTransit(configurator =>
        {
            configurator.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });

            configurator.AddRider(rider =>
            {
                foreach (var action in _riderActions)
                {
                    action(rider);
                }

                rider.UsingKafka((context, k) =>
                {
                    k.Debug = "security,broker";
                    k.CreateKafkaHost(_configuration);

                    foreach (var action in _configurationActions)
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

    private List<Action<IRiderRegistrationConfigurator>> _riderActions = new();
    private List<Action<IKafkaFactoryConfigurator, IRiderRegistrationContext>> _configurationActions = new();

    public CisMessagingKafkaBuilder(CisMessagingBuilder builder, Configuration.IKafkaRiderConfiguration configuration)
    {
        _builder = builder;
        _configuration = configuration;
    }
}
