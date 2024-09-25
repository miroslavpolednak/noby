using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CIS.Infrastructure.WebApi;

/// <summary>
/// Zajisti, aby veskery json output byl html encoded
/// </summary>
public sealed class JsonConverterForStringEncoding 
    : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        if (value != null)
        {
            value = HtmlEncoder.Default.Encode(value);
        }
        writer.WriteStringValue(value);
    }
}
