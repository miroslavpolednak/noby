using Avro.IO;
using Avro.Specific;
using Confluent.Kafka;
using SolTechnology.Avro;

namespace CIS.InternalServices.NotificationService.Mcs.AvroSerializers;

public class AsyncAvroDeserializer<T>: IAsyncDeserializer<T> where T : ISpecificRecord
{
    public Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
    {
        return Task.Run(() =>
        {
            using var ms = new MemoryStream(data.ToArray());
            var dec = new BinaryDecoder(ms);
            var regenObj = (T)Activator.CreateInstance(typeof(T))!;

            var reader = new SpecificDefaultReader(regenObj.Schema, regenObj.Schema);
            reader.Read(regenObj, dec);
            return regenObj;
        });
        
        // return Task.Run(() =>
        //     AvroConvert.Deserialize<T>(data.ToArray())
        //     ?? (T)Activator.CreateInstance(typeof(T))!);
    }
}