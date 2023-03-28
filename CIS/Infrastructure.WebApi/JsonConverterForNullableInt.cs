using System.Text.Json;
using System.Text.Json.Serialization;

namespace CIS.Infrastructure.WebApi;

public sealed class JsonConverterForNullableInt
    : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        else
        {
            return reader.TryGetInt32(out int value) ? value : null;
        }
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (!value.HasValue)
        {
            writer.WriteStringValue("");
        }
        else
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
