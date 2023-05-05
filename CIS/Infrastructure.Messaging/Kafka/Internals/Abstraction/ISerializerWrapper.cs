using Confluent.Kafka;

namespace CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;

public interface ISerializerWrapper
{
    Task<byte[]> SerializeAsync(object data, SerializationContext context);
}