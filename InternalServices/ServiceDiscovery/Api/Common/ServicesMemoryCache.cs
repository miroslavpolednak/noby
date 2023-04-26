using CIS.Core.Types;
using CIS.InternalServices.ServiceDiscovery.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace CIS.InternalServices.ServiceDiscovery.Api.Common;

[Core.Attributes.ScopedService, Core.Attributes.SelfService]
internal sealed class ServicesMemoryCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 20 });
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public static void Clear()
    {
        _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 20 });
    }

    /// <summary>
    /// Pouze pro testovani kese
    /// </summary>
    internal static bool IsKeyInCache(string environmentName)
    {
        return _cache.TryGetValue(new ApplicationEnvironmentName(environmentName), out object? cacheEntry);
    }

    public async Task<IReadOnlyList<Contracts.DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        IReadOnlyList<Contracts.DiscoverableService> cacheEntry;
        if (!_cache.TryGetValue(environmentName, out cacheEntry!))
        {
            _logger.ServicesNotFoundInCache(environmentName);

            SemaphoreSlim mylock = _locks.GetOrAdd(environmentName, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync(cancellationToken);
            try
            {
                if (!_cache.TryGetValue(environmentName, out cacheEntry!))
                {
                    // Key not in cache, so get data.
                    cacheEntry = await getServicesFromDb(environmentName, cancellationToken);
                    _cache.Set(environmentName, cacheEntry, _entryOptions);
                }
            }
            finally
            {
                mylock.Release();
            }
        }
        else
            _logger.ServicesFoundInCache(environmentName);

        return cacheEntry!;
    }

    private async Task<IReadOnlyList<Contracts.DiscoverableService>> getServicesFromDb(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        var list = await _dbContext.ServiceDiscoveryEntities
            .AsNoTracking()
            .Where(t => t.EnvironmentName == environmentName)
            .Select(t => new Dto.ServiceModel
            {
                ServiceName = t.ServiceName,
                ServiceType = (ServiceTypes)t.ServiceType,
                ServiceUrl = t.ServiceUrl
            })
            .ToListAsync(cancellationToken);
        
        if (!list.Any())
        {
            _logger.NoServicesForEnvironment(environmentName);
            throw new CisNotFoundException(103, $"Services not found for environment '{environmentName}'");
        }

        _logger.FoundServices(list.Count, environmentName);

        return list.Select(t => new Contracts.DiscoverableService
            {
                ServiceName = t.ServiceName,
                ServiceType = t.ServiceType,
                ServiceUrl = t.ServiceUrl,
            })
            .ToArray()
            .AsReadOnly();
    }

    static MemoryCacheEntryOptions _entryOptions = new()
    {
        Size = 1,
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
        Priority = CacheItemPriority.High
    };

    private readonly ILogger<ServicesMemoryCache> _logger;
    private readonly Database.ServiceDiscoveryDbContext _dbContext;

    public ServicesMemoryCache(
        ILogger<ServicesMemoryCache> logger,
        Database.ServiceDiscoveryDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
}
