using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace DomainServices.CodebookService.Endpoints;

internal class FastMemoryCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public static async Task<List<TItem>> GetOrCreate<TItem>(string key, Func<Task<List<TItem>>> createItems)
        where TItem : class
    {
        List<TItem> cacheEntry;
        if (!_cache.TryGetValue(key, out cacheEntry))
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    // Key not in cache, so get data.
                    cacheEntry = await createItems();
                    _cache.Set(key, cacheEntry);
                }
            }
            finally
            {
                mylock.Release();
            }
        }
        return cacheEntry;
    }
}
