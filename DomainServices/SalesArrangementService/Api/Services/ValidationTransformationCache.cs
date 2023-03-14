using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;

namespace DomainServices.SalesArrangementService.Api.Services;

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

    public sealed class TransformationItem
    {
        public int? CategoryOrder { get; init; }
        public string? Category { get; init; }
        public string Text { get; init; } = string.Empty;
        public Database.FormValidationTransformationAlterSeverity AlterSeverity { get; init; }
    }

    internal static IReadOnlyDictionary<string, TransformationItem> GetOrCreate(int key, Func<IReadOnlyDictionary<string, TransformationItem>> createItems)
    {
        IReadOnlyDictionary<string, TransformationItem> cacheEntry;
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
