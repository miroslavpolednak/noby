namespace CIS.InternalServices.ServiceDiscovery.Clients;

public interface IDiscoveryServiceClient
{
    Task<IReadOnlyList<DiscoverableService>> GetServices(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken = default);

    Task<DiscoverableService> GetService(ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default);

    Task<DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default);

    string GetServiceUrlSynchronously(ApplicationKey serviceName, Contracts.ServiceTypes serviceType);

    IReadOnlyList<DiscoverableService> GetServicesSynchronously();
}
