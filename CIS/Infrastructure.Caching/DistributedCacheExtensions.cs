using CIS.Core.Configuration;
using CIS.Infrastructure.StartupExtensions;
using Microsoft.Extensions.Caching.Distributed;
using ProtoBuf;
using System.Text.Json;

namespace CIS.Infrastructure.Caching;

public static class DistributedCacheExtensions
{
    public static async Task<TModel?> GetObjectAsync<TModel>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default(CancellationToken))
        => await cache.GetObjectAsync<TModel?>(key, DistributedCachingStartupExtensions.Configuration.SerializationType, cancellationToken);

    public static async Task SetObjectAsync<TModel>(this IDistributedCache cache, string key, TModel value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default(CancellationToken))
        => await cache.SetObjectAsync<TModel>(key, value, options, DistributedCachingStartupExtensions.Configuration.SerializationType, cancellationToken);

    public static async Task<TModel?> GetObjectAsync<TModel>(this IDistributedCache cache, string key, ICisDistributedCacheConfiguration.SerializationTypes serializationType, CancellationToken cancellationToken = default(CancellationToken))
    {
        var data = await cache.GetAsync(key, cancellationToken);
        if (data is null) return default(TModel);

        switch (serializationType)
        {
            case ICisDistributedCacheConfiguration.SerializationTypes.Protobuf:
                using (var ms = new MemoryStream(data))
                {
                    return Serializer.Deserialize<TModel>(ms);
                }

            case ICisDistributedCacheConfiguration.SerializationTypes.Json:
            default:
                return JsonSerializer.Deserialize<TModel>(data);
        }
    }

    public static async Task SetObjectAsync<TModel>(this IDistributedCache cache, string key, TModel value, DistributedCacheEntryOptions options, ICisDistributedCacheConfiguration.SerializationTypes serializationType, CancellationToken cancellationToken = default(CancellationToken))
    {
        switch (serializationType)
        {
            case ICisDistributedCacheConfiguration.SerializationTypes.Protobuf:
                using (var ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, value);
                    await cache.SetAsync(key, ms.ToArray(), options, cancellationToken);
                }
                break;

            case ICisDistributedCacheConfiguration.SerializationTypes.Json:
            default:
                await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), options, cancellationToken);
                break;
        }
    }
}
