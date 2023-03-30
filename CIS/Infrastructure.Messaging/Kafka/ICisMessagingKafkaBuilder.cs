using MassTransit;

namespace CIS.Infrastructure.Messaging.Kafka;

public interface ICisMessagingKafkaBuilder
{
    ICisMessagingKafkaBuilder AddConsumers(Action<IRiderRegistrationConfigurator> action);

    ICisMessagingKafkaBuilder AddProducers(Action<IRiderRegistrationConfigurator> action);

    ICisMessagingKafkaBuilder AddConsumersToTopic(Action<IKafkaFactoryConfigurator, IRiderRegistrationContext> action);

    ICisMessagingBuilder Build();
}
