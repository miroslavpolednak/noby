using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.SchemaRegistry;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.SchemaRegistry;

internal class SchemaTypeConverter : JsonConverter<SchemaType>
{
    public override SchemaType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string @string = reader.GetString();
        if (Enum.TryParse<SchemaType>(@string, ignoreCase: true, out var result))
        {
            return result;
        }

        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, SchemaType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}