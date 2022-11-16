using DomainServices.SalesArrangementService.Api.Handlers.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DomainServices.SalesArrangementService.Api.Handlers.Services;

internal sealed class ValidationTransformationService
{
    private static Regex _arrayIndexesRegex = new Regex(@"/(\[\d\])/g", RegexOptions.Compiled);

    public List<Contracts.ValidationMessage> TransformErrors(string formId, Form form, Dictionary<string, ExternalServices.Eas.R21.CheckFormV2.Error[]>? errors)
    {
        if (errors is null || !errors.Any()) return new List<Contracts.ValidationMessage>(0);

        var transformedItems = new List<Contracts.ValidationMessage>(errors.Count);
        // get transformation values from DB
        ValidationTransformationCache.GetOrCreate(formId, () =>
        {
            _dbContext.FormValidationTransformations
            .AsNoTracking()
            .Where(t => t.FormId == formId)
            .Select(t => new  )
        })

        foreach (var errorGroup in errors)
        {
            foreach (var error in errorGroup.Value)
            {
                // kopie chyby SB
                var item = new Contracts.ValidationMessage
                {
                    AdditionalInformation = error.AdditionalInformation,
                    Code = error.ErrorCode,
                    ErrorQueue = error.ErrorQueue,
                    Message = error.ErrorMessage,
                    Severity= error.Severity,
                    Value = error.Value,
                    Parameter = errorGroup.Key
                };

                // transformace na NOBY chybu
                var matches = _arrayIndexesRegex.Matches(errorGroup.Key);
                if (matches.Any())
                {
                }
                else
                {

                }

                transformedItems.Add(item);
            }
        }

        return transformedItems;
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public ValidationTransformationService(Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}

internal sealed class ValidationTransformationCache
{
    //TODO zatim se mi to nechce datavat do appsettings
    public const int AbsoluteExpirationInMinutes = 10;

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
        public string Category { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }

    internal static async Task<ImmutableDictionary<TransformationItem>> GetOrCreate<TItem>(string key, Func<Task<List<TransformationItem>>> createItems)
    {
        List<TransformationItem> cacheEntry;
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