using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace DomainServices.CodebookService.Clients;

internal sealed class ClientsMemoryCache : IDisposable
{
    private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();
    private static CancellationTokenSource _changeTokenSource = new CancellationTokenSource();

    public static void Reset()
    {
        _changeTokenSource.Cancel();
        _changeTokenSource = new CancellationTokenSource();
    }

    public async Task<TItem> GetOrCreate<TItem>(Func<Task<TItem>> createItem, [CallerMemberName] string method = "")
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        if (!_cache.TryGetValue(method, out TItem cacheEntry))
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(method, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();
            try
            {
                if (!_cache.TryGetValue(method, out cacheEntry))
                {
                    cacheEntry = await createItem();

                    // opts
                    var cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(StartupExtensions.DefaultAbsoluteCacheExpirationMinutes)
                    };
                    cacheOptions.AddExpirationToken(new CancellationChangeToken(_changeTokenSource.Token));

                    _cache.Set(method, cacheEntry, cacheOptions);
                }
            }
            finally
            {
                mylock.Release();
            }
        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        return cacheEntry!;
    }

    public void Dispose()
    {
        if (_cache is not null)
        {
            _cache.Dispose();
        }
    }
}
