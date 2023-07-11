using System.Reflection;

namespace CIS.Infrastructure.Messaging;

public interface ICisMessagingBuilder
{
    Kafka.ICisMessagingKafkaBuilder AddKafka(Assembly? assembly = null);
}
