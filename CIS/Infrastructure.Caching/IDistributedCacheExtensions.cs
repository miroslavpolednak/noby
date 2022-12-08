using Microsoft.Extensions.Caching.Distributed;
using ProtoBuf;
using System.Text.Json;

namespace CIS.Infrastructure.Caching;

public static class IDistributedCacheExtensions
{
    public static async Task<TModel?> GetObjectAsync<TModel>(this IDistributedCache cache, string key, SerializationTypes serializationType = SerializationTypes.Default, CancellationToken cancellationToken = default(CancellationToken))
    {
        var data = await cache.GetAsync(key, cancellationToken);
        if (data is null) return default(TModel);

        switch (serializationType)
        {
            case SerializationTypes.Protobuf:
                using (var ms = new MemoryStream(data))
                {
                    return Serializer.Deserialize<TModel>(ms);
                }

            case SerializationTypes.Json:
            default:
                return JsonSerializer.Deserialize<TModel>(data);
        }
    }

    public static async Task SetObjectAsync<TModel>(this IDistributedCache cache, string key, TModel value, DistributedCacheEntryOptions options, SerializationTypes serializationType = SerializationTypes.Default, CancellationToken cancellationToken = default(CancellationToken))
    {
        switch (serializationType)
        {
            case SerializationTypes.Protobuf:
                using (var ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, value);
                    await cache.SetAsync(key, ms.ToArray(), options, cancellationToken);
                }
                break;

            case SerializationTypes.Json:
            default:
                await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), options, cancellationToken);
                break;
        }
    }
}
