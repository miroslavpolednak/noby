namespace CIS.Infrastructure.Messaging;

public interface ICisMessagingBuilder
{
    Kafka.ICisMessagingKafkaBuilder AddKafka();
}
