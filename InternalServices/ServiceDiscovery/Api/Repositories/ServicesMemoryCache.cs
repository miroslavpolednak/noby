using CIS.Core.Types;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace CIS.InternalServices.ServiceDiscovery.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class ServicesMemoryCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public async Task<List<Contracts.DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        List<Contracts.DiscoverableService> cacheEntry;
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
                    _cache.Set(environmentName, cacheEntry);
                }
            }
            finally
            {
                mylock.Release();
            }
        }
        else
            _logger.ServicesFoundInCache(environmentName);

        return cacheEntry;
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
