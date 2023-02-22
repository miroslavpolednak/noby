using Avro.IO;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Infrastructure;

public interface IReaderWrapper
{
    object Read(BinaryDecoder decoder);
}