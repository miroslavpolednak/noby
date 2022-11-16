using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace DomainServices.SalesArrangementService.Api.Handlers.Services;

internal static class ValidationTransformationCache
{
    const int AbsoluteExpirationInMinutes = 10;

    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();
    private static CancellationTokenSource _changeTokenSource = new CancellationTokenSource();

    public static void Reset()
    {
        _changeTokenSource.Cancel();
        _changeTokenSource = new CancellationTokenSource();
    }

    public class TransformationItem
    {
        public string Name { get; init; } = string.Empty;
        public string? Category { get; init; }
        public string Text { get; init; } = string.Empty;
        public Repositories.FormValidationTransformationAlterSeverity AlterSeverity { get; init; }
    }

    internal static ImmutableDictionary<string, TransformationItem> GetOrCreate(string key, Func<ImmutableDictionary<string, TransformationItem>> createItems)
    {
        ImmutableDictionary<string, TransformationItem> cacheEntry;
        if (!_cache.TryGetValue(key, out cacheEntry))
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            mylock.Wait();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    // Key not in cache, so get data.
                    cacheEntry = createItems();

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
