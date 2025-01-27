﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CIS.Infrastructure.WebApi;

public sealed class JsonConverterForNullableDateTime
    : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        else
        {
            return reader.TryGetDateTime(out DateTime d) ? d : null;
        }
    }

    // This method will be ignored on serialization, and the default typeof(DateTime) converter is used instead.
    // This is a bug: https://github.com/dotnet/corefx/issues/41070#issuecomment-560949493
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (!value.HasValue)
        {
            writer.WriteStringValue("");
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
        }
    }
}
