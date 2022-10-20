using Avro.IO;
using Avro.Specific;
using Confluent.Kafka;
using SolTechnology.Avro;

namespace CIS.InternalServices.NotificationService.Msc.AvroSerializers;

public class AsyncAvroDeserializer<T>: IAsyncDeserializer<T> where T : ISpecificRecord
{
    public Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
    {
        return Task.Run(() =>
            AvroConvert.Deserialize<T>(data.ToArray())
            ?? (T)Activator.CreateInstance(typeof(T))!);
    }
}