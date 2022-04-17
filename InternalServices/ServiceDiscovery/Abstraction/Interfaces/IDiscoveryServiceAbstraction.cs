using CIS.Core.Types;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

public interface IDiscoveryServiceAbstraction
{
    Task<IReadOnlyCollection<DiscoverableService>> GetServices(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken);

    Task<DiscoverableService> GetService(ServiceName serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken);

    Task<DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ServiceName serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken);
}
