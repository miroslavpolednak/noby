using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;

namespace DomainServices.CodebookService.Api.Database;

internal sealed class CodebookMemoryCache
{
    private static readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private static readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new();
    private static CancellationTokenSource _changeTokenSource = new();

    //TODO zatim se mi to nechce datavat do appsettings
    public const int AbsoluteExpirationInMinutes = 1;

    private static MemoryCacheEntryOptions getCacheOptions()
    {
        return (new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(AbsoluteExpirationInMinutes)
        })
        .AddExpirationToken(new CancellationChangeToken(_changeTokenSource.Token));
    }
    
    public static void Reset()
    {
        _changeTokenSource.Cancel();
        _changeTokenSource = new CancellationTokenSource();
    }

    public static TResponse GetOrCreate<TResponse>(string key, Func<TResponse> createItems)
        where TResponse : class
    {
        if (!_cache.TryGetValue(key, out TResponse? cacheEntry))
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            mylock.Wait();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    cacheEntry = createItems();
                    _cache.Set(key, cacheEntry, getCacheOptions());
                }
            }
            finally
            {
                mylock.Release();
            }
        }
        return cacheEntry!;
    }

    public static TResponse GetOrCreate<TResponse>(string key, ReadOnlySpan<char> sqlQuery, Func<string, TResponse> createItems)
        where TResponse : class
    {
        if (!_cache.TryGetValue(key, out TResponse? cacheEntry))
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            mylock.Wait();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    cacheEntry = createItems(sqlQuery.ToString());
                    _cache.Set(key, cacheEntry, getCacheOptions());
                }
            }
            finally
            {
                mylock.Release();
            }
        }
        return cacheEntry!;
    }
}
