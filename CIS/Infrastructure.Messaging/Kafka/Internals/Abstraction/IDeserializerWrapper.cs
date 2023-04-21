using Confluent.Kafka;

namespace CIS.Infrastructure.Messaging.Kafka.Internals.Abstraction;

public interface IDeserializerWrapper
{
    Task<object> DeserializeAsync(byte[] data, SerializationContext context);
}