namespace CIS.InternalServices.ServiceDiscovery.Clients;

internal sealed class DiscoveryServiceClient 
    : IDiscoveryServiceClient
{
    public async Task<ImmutableList<DiscoverableService>> GetServices(CancellationToken cancellationToken = default(CancellationToken))
        => await GetServices(getEnvName(), cancellationToken);

    // get services nekesujeme, nemel by to byt casty dotaz
    public async Task<ImmutableList<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken = default(CancellationToken))
        => await _cache.GetServices(environmentName, cancellationToken);
    
    public async Task<DiscoverableService> GetService(ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken))
        => await GetService(getEnvName(), serviceName, serviceType, cancellationToken);

    public async Task<DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken))
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

    public ImmutableList<DiscoverableService> GetServicesSynchronously()
    {
        return _cache.GetServices(getEnvName(), default(CancellationToken)).GetAwaiter().GetResult();
    }

    private readonly ServicesMemoryCache _cache;
    private readonly EnvironmentNameProvider _envName;

    public DiscoveryServiceClient(
        ServicesMemoryCache cache,
        EnvironmentNameProvider envName)
    {
        _cache = cache;
        _envName = envName;
    }

    private ApplicationEnvironmentName getEnvName()
        => _envName.Name ?? throw new InvalidOperationException("EnvironmentName");
}
