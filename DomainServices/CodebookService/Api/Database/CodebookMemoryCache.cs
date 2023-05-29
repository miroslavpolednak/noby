using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;

namespace DomainServices.CodebookService.Api.Database;

internal sealed class CodebookMemoryCache
{
    //TODO zatim se mi to nechce datavat do appsettings
    public const int AbsoluteExpirationInMinutes = 10;

    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();
    private static CancellationTokenSource _changeTokenSource = new CancellationTokenSource();

    private static MemoryCacheEntryOptions _cacheOptions = (new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(AbsoluteExpirationInMinutes)
        })
        .AddExpirationToken(new CancellationChangeToken((new CancellationTokenSource()).Token));
    
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
                    _cache.Set(key, createItems(), _cacheOptions);
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
                    _cache.Set(key, createItems(sqlQuery.ToString()), _cacheOptions);
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
