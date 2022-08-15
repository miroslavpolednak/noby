using System.Text.Json.Serialization;
using System.Text.Json;

namespace CIS.Infrastructure.Json;

public sealed class DateTimeOffsetConverterUsingDateTimeParse 
    : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
#pragma warning disable CA1305 // Specify IFormatProvider
#pragma warning disable CS8604 // Possible null reference argument.
        return DateTimeOffset.Parse(reader.GetString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CA1305 // Specify IFormatProvider
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
#pragma warning disable CA1305 // Specify IFormatProvider
        writer.WriteStringValue(value.ToString());
#pragma warning restore CA1305 // Specify IFormatProvider
    }
}
