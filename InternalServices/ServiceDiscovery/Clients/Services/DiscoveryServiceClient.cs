﻿namespace CIS.InternalServices.ServiceDiscovery.Clients;

internal sealed class DiscoveryServiceClient(
    ServicesMemoryCache _cache,
    EnvironmentNameProvider _envName)
    : IDiscoveryServiceClient
{
    public async Task<IReadOnlyList<DiscoverableService>> GetServices(CancellationToken cancellationToken = default)
        => await GetServices(getEnvName(), cancellationToken);

    // get services nekesujeme, nemel by to byt casty dotaz
    public async Task<IReadOnlyList<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken = default)
        => await _cache.GetServices(environmentName, cancellationToken);
    
    public async Task<DiscoverableService> GetService(ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default)
        => await GetService(getEnvName(), serviceName, serviceType, cancellationToken);

    public async Task<DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default)
    {
        var services = await _cache.GetServices(environmentName, cancellationToken);
        return services.FirstOrDefault(t => t.ServiceName == serviceName && t.ServiceType == serviceType) 
            ?? throw new CisNotFoundException(0, $"Service {serviceName}:{serviceType} not found");
    }

    public string GetServiceUrlSynchronously(ApplicationKey serviceName, Contracts.ServiceTypes serviceType)
    {
        var serviceFromCache = ServicesMemoryCache.GetServiceFromCache(getEnvName(), serviceName, serviceType);
        if (serviceFromCache is null)
        {
            var url = GetService(serviceName, serviceType).GetAwaiter().GetResult()?.ServiceUrl;
            if (string.IsNullOrEmpty(url))
                throw new CisNotFoundException(0, $"Service Discovery can not find {serviceName}{serviceType} service URL");
            return url;
        }
        else
            return serviceFromCache.ServiceUrl;
    }

    public IReadOnlyList<DiscoverableService> GetServicesSynchronously()
    {
        return _cache.GetServices(getEnvName(), default).GetAwaiter().GetResult();
    }

    private ApplicationEnvironmentName getEnvName()
        => _envName.Name ?? throw new InvalidOperationException("EnvironmentName");
}
