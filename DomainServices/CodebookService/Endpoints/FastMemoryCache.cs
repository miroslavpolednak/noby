using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;

namespace DomainServices.CodebookService.Endpoints;

public sealed class FastMemoryCache
{
    //TODO zatim se mi to nechce datavat do appsettings
    public const int AbsoluteExpirationInMinutes = 1;

    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();
    private static CancellationTokenSource _changeTokenSource = new CancellationTokenSource();

    public static void Reset()
    {
        _changeTokenSource.Cancel();
        _changeTokenSource = new CancellationTokenSource();
    }

    internal static async Task<List<TItem>> GetOrCreate<TItem>(string key, Func<Task<List<TItem>>> createItems)
        where TItem : class
    {
        if (!_cache.TryGetValue(key, out List<TItem>? cacheEntry))
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    // Key not in cache, so get data.
                    cacheEntry = await createItems();

                    // opts
                    var cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(AbsoluteExpirationInMinutes)
                    };
                    cacheOptions.AddExpirationToken(new CancellationChangeToken(_changeTokenSource.Token));

                    _cache.Set(key, cacheEntry, cacheOptions);
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
