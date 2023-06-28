using Grpc.Net.Client;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints.GlobalHealtCheck;

internal sealed class GrpcChannelsCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 20 });
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public static void RemoveChannel(string serviceName)
    {
        _cache.Remove(serviceName);
    }

    public static GrpcChannel? GetChannel(Contracts.DiscoverableService service)
    {
        if (!_cache.TryGetValue(service.ServiceName, out GrpcChannel? cacheEntry))
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(service.ServiceName, k => new SemaphoreSlim(1, 1));

            mylock.Wait();
            try
            {
                if (!_cache.TryGetValue(service.ServiceName, out cacheEntry))
                {
                    // Key not in cache, so get data.
                    cacheEntry = createAndSetGrpcChannel(service);
                }
            }
            finally
            {
                mylock.Release();
            }
        }

        return cacheEntry;
    }

    private static GrpcChannel createAndSetGrpcChannel(Contracts.DiscoverableService service)
    {
        var cacheEntry = GrpcChannel.ForAddress(service.ServiceUrl, new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            }
        });
        _cache.Set(service.ServiceName, cacheEntry, _entryOptions);
        return cacheEntry;
    }

    private static MemoryCacheEntryOptions _entryOptions = new()
    {
        Size = 1,
        SlidingExpiration = TimeSpan.FromMinutes(10),
        Priority = CacheItemPriority.High
    };
}
