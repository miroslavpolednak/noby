using Avro.IO;
using Avro.Specific;
using Confluent.Kafka;
using SolTechnology.Avro;

namespace CIS.InternalServices.NotificationService.Msc.AvroSerializers;

public class AsyncAvroSerializer<T>: IAsyncSerializer<T> where T : ISpecificRecord
{
    public Task<byte[]> SerializeAsync(T data, SerializationContext context)
    {
        return Task.Run(() => AvroConvert.Serialize(data));
    }
}