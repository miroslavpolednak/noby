using Confluent.Kafka;

namespace Mock.Mcs.Messaging.Infrastructure;

public interface ISerializerWrapper
{
    Task<byte[]> SerializeAsync(object data, SerializationContext context);
}