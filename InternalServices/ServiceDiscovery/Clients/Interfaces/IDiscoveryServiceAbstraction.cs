using CIS.Core.Types;

namespace CIS.InternalServices.ServiceDiscovery.Clients;

public interface IDiscoveryServiceAbstraction
{
    Task<IList<DiscoverableService>> GetServices(CancellationToken cancellationToken = default(CancellationToken));

    Task<IList<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken = default(CancellationToken));

    Task<DiscoverableService> GetService(ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken));

    Task<DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken));

    string GetServiceUrlSynchronously(ApplicationKey serviceName, Contracts.ServiceTypes serviceType);
}
