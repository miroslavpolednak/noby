using Google.Protobuf;
using Newtonsoft.Json;

namespace CIS.Infrastructure.CisMediatR.PayloadLogger;

internal sealed class JsonConvertByteString
    : JsonConverter<ByteString>
{
    public override ByteString ReadJson(JsonReader reader, Type objectType, ByteString? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return ByteString.FromBase64("");
    }

    public override void WriteJson(JsonWriter writer, ByteString? value, JsonSerializer serializer)
    {
        writer.WriteValue("--byte array--");
    }
}
