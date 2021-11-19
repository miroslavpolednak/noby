using System.Text.Encodings.Web;
using System.Text.Json;

namespace CIS.Infrastructure.Caching.Redis
{
    internal static class SerializationOptions
    {
        static SerializationOptions()
        {
            Flexible = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Always,
                WriteIndented = false,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        public static JsonSerializerOptions Flexible { get; }
    }
}
