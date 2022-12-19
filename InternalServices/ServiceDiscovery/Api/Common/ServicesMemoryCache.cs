using CIS.Core.Types;
using CIS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Collections.Immutable;

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

    public async Task<ImmutableList<Contracts.DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        ImmutableList<Contracts.DiscoverableService> cacheEntry;
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

    private async Task<ImmutableList<Contracts.DiscoverableService>> getServicesFromDb(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        var list = await _connectionProvider
            .ExecuteDapperRawSqlToList<Dto.ServiceModel>(_sqlQuery, new { name = environmentName.ToString() }, cancellationToken);

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
            .ToImmutableList();
    }

    const string _sqlQuery = "SELECT ServiceName, ServiceUrl, ServiceType FROM ServiceDiscovery WHERE EnvironmentName=@name";

    static MemoryCacheEntryOptions _entryOptions = new()
    {
        Size = 1,
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
        Priority = CacheItemPriority.High
    };

    private readonly ILogger<ServicesMemoryCache> _logger;
    private readonly Core.Data.IConnectionProvider _connectionProvider;

    public ServicesMemoryCache(
        Core.Data.IConnectionProvider connectionProvider,
        ILogger<ServicesMemoryCache> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
