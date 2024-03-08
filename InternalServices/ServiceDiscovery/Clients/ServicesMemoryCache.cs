﻿using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using Polly;

namespace CIS.InternalServices.ServiceDiscovery.Clients;

internal sealed class ServicesMemoryCache
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public static DiscoverableService? GetServiceFromCache(ApplicationEnvironmentName environmentName, ApplicationKey serviceName, Contracts.ServiceTypes serviceType)
    {
        if (_cache.TryGetValue(environmentName, out IReadOnlyList<DiscoverableService>? cacheEntry))
        {
            return cacheEntry?.FirstOrDefault(t => t.ServiceName == serviceName && t.ServiceType == serviceType);
        }
        return null;
    }

    public async Task<IReadOnlyList<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        IReadOnlyList<DiscoverableService>? cacheEntry;
        if (!_cache.TryGetValue(environmentName, out cacheEntry))
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(environmentName, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync(cancellationToken);
            try
            {
                if (!_cache.TryGetValue(environmentName, out cacheEntry))
                {
                    var retryPolicy = Policy<IReadOnlyList<DiscoverableService>>.Handle<Exception>().WaitAndRetryAsync(18, _ => TimeSpan.FromSeconds(10));

                    // Key not in cache, so get data.
                    cacheEntry = await retryPolicy.ExecuteAsync(() => getServicesFromRemote(environmentName, cancellationToken));
                    _cache.Set(environmentName, cacheEntry, new MemoryCacheEntryOptions
                    {
                        Size = 1,
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                        Priority = CacheItemPriority.High
                    });
                }
            }
            finally
            {
                mylock.Release();
            }
        }

        return cacheEntry!;
    }

    private async Task<IReadOnlyList<DiscoverableService>> getServicesFromRemote(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken)
    {
        var result = await _service.GetServicesAsync(
            new Contracts.GetServicesRequest
            {
                Environment = environmentName
            }, cancellationToken: cancellationToken);
        
        return result
            .Services
            .Select(t => new DiscoverableService(t.ServiceName, t.ServiceUrl, t.ServiceType))
            .ToArray()
            .AsReadOnly();
    }

    private readonly Contracts.v1.DiscoveryService.DiscoveryServiceClient _service;

    public ServicesMemoryCache(Contracts.v1.DiscoveryService.DiscoveryServiceClient service)
    {
        _service = service;
    }
}
