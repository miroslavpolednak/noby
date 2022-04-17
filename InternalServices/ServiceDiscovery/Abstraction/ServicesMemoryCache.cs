using CIS.Core.Types;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

internal class ServicesMemoryCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 50 });
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public async Task<IReadOnlyCollection<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<DiscoverableService> cacheEntry;
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

    private async Task<IReadOnlyCollection<DiscoverableService>> getServicesFromRemote(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStarted(nameof(GetServices));

        var result = await _userContext.AddUserContext(async () => await _service.GetServicesAsync(
            new Contracts.GetServicesRequest
            {
                Environment = environmentName
            }, cancellationToken: cancellationToken)
        );
        
        return result
            .Services
            .Select(t => new DiscoverableService(t.ServiceName, t.ServiceUrl, t.ServiceType)).ToList()
            .AsReadOnly();
    }

    private readonly ILogger<ServicesMemoryCache> _logger;
    private readonly Security.InternalServices.ICisUserContextHelpers _userContext;
    private readonly Contracts.v1.DiscoveryService.DiscoveryServiceClient _service;

    public ServicesMemoryCache(
        Contracts.v1.DiscoveryService.DiscoveryServiceClient service,
        Security.InternalServices.ICisUserContextHelpers userContext,
        ILogger<ServicesMemoryCache> logger)
    {
        _logger = logger;
        _service = service;
        _userContext = userContext;
    }
}
