using System.Text.Json;

namespace FOMS.DocumentContracts;

public abstract class BaseContract
{
    protected static JsonSerializerOptions _jsonSerializationOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    protected TPart castObjectToPart<TPart>(object data)
        => JsonSerializer.Deserialize<TPart>(data.ToString() ?? "", _jsonSerializationOptions) ?? throw new Exception("data object is empty");
}
