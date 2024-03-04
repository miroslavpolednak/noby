using System.Reflection;
using CIS.Infrastructure.Messaging.Kafka;
using CIS.Infrastructure.Messaging.KafkaFlow;

namespace CIS.Infrastructure.Messaging;

public interface ICisMessagingBuilder
{
    ICisMessagingKafkaBuilder AddKafka(Assembly? assembly = null);
    ICisMessagingBuilder AddKafkaFlow(Action<IKafkaFlowMessagingConfigurator> messaging);
}
