using Confluent.Kafka;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Infrastructure;

public interface ISerializerWrapper
{
    Task<byte[]> SerializeAsync(object data, SerializationContext context);
}