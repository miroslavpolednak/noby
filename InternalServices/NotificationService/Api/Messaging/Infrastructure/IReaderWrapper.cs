using Avro.IO;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Infrastructure;

public interface IReaderWrapper
{
    object Read(BinaryDecoder decoder);
}