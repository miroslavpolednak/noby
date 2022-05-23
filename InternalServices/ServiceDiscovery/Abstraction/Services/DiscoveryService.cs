using CIS.Core.Types;
using System.Collections.Immutable;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

internal sealed class DiscoveryService : IDiscoveryServiceAbstraction
{
    public async Task<ImmutableList<DiscoverableService>> GetServices(CancellationToken cancellationToken = default(CancellationToken))
        => await GetServices(getEnvName(), cancellationToken);

    // get services nekesujeme, nemel by to byt casty dotaz
    public async Task<ImmutableList<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(GetServices));

        return await _cache.GetServices(environmentName, cancellationToken);
    }
    
    public async Task<DiscoverableService> GetService(ServiceName serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken))
        => await GetService(getEnvName(), serviceName, serviceType, cancellationToken);

    public async Task<DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ServiceName serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.GetServiceStarted(serviceName, serviceType, environmentName.Name);

        var services = await _cache.GetServices(environmentName, cancellationToken);
        return services.FirstOrDefault(t => t.ServiceName == serviceName && t.ServiceType == serviceType) 
            ?? throw new CIS.Core.Exceptions.CisNotFoundException(0, $"Service {serviceName}:{serviceType} not found");
    }

    public string GetServiceUrlSynchronously(ServiceName serviceName, Contracts.ServiceTypes serviceType)
    {
        var serviceFromCache = ServicesMemoryCache.GetServiceFromCache(getEnvName(), serviceName, serviceType);
        if (serviceFromCache is null)
        {
            var url = GetService(serviceName, serviceType).GetAwaiter().GetResult()?.ServiceUrl;
            if (string.IsNullOrEmpty(url))
                throw new CIS.Core.Exceptions.CisNotFoundException(0, $"Service Discovery can not find {serviceName}{serviceType} service URL");
            return url;
        }
        else
            return serviceFromCache.ServiceUrl;
    }

    private readonly ServicesMemoryCache _cache;
    private readonly ILogger<DiscoveryService> _logger;
    private readonly EnvironmentNameProvider _envName;

    public DiscoveryService(
        ServicesMemoryCache cache,
        ILogger<DiscoveryService> logger,
        EnvironmentNameProvider envName)
    {
        _logger = logger;
        _cache = cache;
        _envName = envName;
    }

    private ApplicationEnvironmentName getEnvName()
        => _envName.Name ?? throw new InvalidOperationException("EnvironmentName");
}
