using Avro.Specific;
using Confluent.Kafka;
using SolTechnology.Avro;

namespace CIS.InternalServices.NotificationService.Mcs.AvroSerializers;

public class AsyncAvroDeserializer<T>: IAsyncDeserializer<T> where T : ISpecificRecord
{
    public async Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
    {
        return await Task.Run(() =>
        {
            var defaultObject = (T)Activator.CreateInstance(typeof(T))!;
            var schema = defaultObject.Schema.ToString();
            return AvroConvert.DeserializeHeadless<T>(data.ToArray(), schema);
        });
    }
}