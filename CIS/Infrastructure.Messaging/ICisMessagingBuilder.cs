using CIS.Infrastructure.Messaging.KafkaFlow;

namespace CIS.Infrastructure.Messaging;

public interface ICisMessagingBuilder
{
    ICisMessagingBuilder AddKafkaFlow(Action<IKafkaFlowMessagingConfigurator> messaging);
    ICisMessagingBuilder AddKafkaFlowDashboard();
}
