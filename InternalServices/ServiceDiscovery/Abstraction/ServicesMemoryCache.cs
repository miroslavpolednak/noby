using CIS.Core.Types;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

internal sealed class ServicesMemoryCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public async Task<ImmutableList<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        ImmutableList<DiscoverableService> cacheEntry;
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
                    cacheEntry = await getServicesFromRemote(environmentName, cancellationToken);
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

    private async Task<ImmutableList<DiscoverableService>> getServicesFromRemote(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        var result = await _userContext.AddUserContext(async () => await _service.GetServicesAsync(
            new Contracts.GetServicesRequest
            {
                Environment = environmentName
            }, cancellationToken: cancellationToken)
        );
        
        return result
            .Services
            .Select(t => new DiscoverableService(t.ServiceName, t.ServiceUrl, t.ServiceType))
            .ToImmutableList();
    }

    private readonly ILogger<ServicesMemoryCache> _logger;
    private readonly DomainServicesSecurity.Abstraction.ICisUserContextHelpers _userContext;
    private readonly Contracts.v1.DiscoveryService.DiscoveryServiceClient _service;

    public ServicesMemoryCache(
        Contracts.v1.DiscoveryService.DiscoveryServiceClient service,
        DomainServicesSecurity.Abstraction.ICisUserContextHelpers userContext,
        ILogger<ServicesMemoryCache> logger)
    {
        _logger = logger;
        _service = service;
        _userContext = userContext;
    }
}
