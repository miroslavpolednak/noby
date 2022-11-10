using Avro.Specific;
using Confluent.Kafka;
using SolTechnology.Avro;

namespace CIS.InternalServices.NotificationService.Mcs.AvroSerializers;

public class AsyncAvroSerializer<T>: IAsyncSerializer<T> where T : ISpecificRecord
{
    public Task<byte[]> SerializeAsync(T data, SerializationContext context)
    {
        return Task.Run(() =>
        {
            var schema = data.Schema.ToString();
            return AvroConvert.SerializeHeadless(data, schema);
        });
    }
}