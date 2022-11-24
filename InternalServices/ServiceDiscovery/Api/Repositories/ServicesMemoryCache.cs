using CIS.Core.Types;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace CIS.InternalServices.ServiceDiscovery.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class ServicesMemoryCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 20 });
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public static void Clear()
    {
        _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 20 });
    }

    public async Task<List<Contracts.DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        List<Contracts.DiscoverableService>? cacheEntry;
        if (!_cache.TryGetValue(environmentName, out cacheEntry))
        {
            _logger.ServicesNotFoundInCache(environmentName);

            SemaphoreSlim mylock = _locks.GetOrAdd(environmentName, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync(cancellationToken);
            try
            {
                if (!_cache.TryGetValue(environmentName, out cacheEntry))
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

        return cacheEntry ?? new List<Contracts.DiscoverableService>(0);
    }

    private async Task<List<Contracts.DiscoverableService>> getServicesFromDb(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        var list = await _repository.GetList(environmentName, cancellationToken);

        if (!list.Any())
        {
            _logger.NoServicesForEnvironment(environmentName);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Services not found for environment '{environmentName}'", 103);
        }

        _logger.FoundServices(list.Count, environmentName);

        return list.Select(t => new Contracts.DiscoverableService
            {
                ServiceName = t.ServiceName,
                ServiceType = t.ServiceType,
                ServiceUrl = t.ServiceUrl,
            })
            .ToList();
    }

    static MemoryCacheEntryOptions _entryOptions = new()
    {
        Size = 1,
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
        Priority = CacheItemPriority.High
    };

    private readonly ILogger<ServicesMemoryCache> _logger;
    private readonly ServiceDiscoveryRepository _repository;

    public ServicesMemoryCache(
        ServiceDiscoveryRepository repository,
        ILogger<ServicesMemoryCache> logger)
    {
        _logger = logger;
        _repository = repository;
    }
}
