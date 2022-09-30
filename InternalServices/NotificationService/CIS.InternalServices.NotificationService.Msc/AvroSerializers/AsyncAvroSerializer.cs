using Avro.IO;
using Avro.Specific;
using Confluent.Kafka;

namespace CIS.InternalServices.NotificationService.Msc.AvroSerializers;

public class AsyncAvroSerializer<T>: IAsyncSerializer<T> where T : ISpecificRecord
{
    public Task<byte[]> SerializeAsync(T data, SerializationContext context)
    {
        return Task.Run(() =>
        {
            using var ms = new MemoryStream();
            var enc = new BinaryEncoder(ms);
            var writer = new SpecificDefaultWriter(data.Schema);
            writer.Write(data, enc);
            return ms.ToArray();
        });
    }
}