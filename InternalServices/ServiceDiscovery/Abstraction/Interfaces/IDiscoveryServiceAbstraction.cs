using CIS.Core.Types;
using System.Collections.Immutable;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

public interface IDiscoveryServiceAbstraction
{
    Task<ImmutableList<DiscoverableService>> GetServices(CancellationToken cancellationToken = default(CancellationToken));

    Task<ImmutableList<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken = default(CancellationToken));

    Task<DiscoverableService> GetService(ServiceName serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken));

    Task<DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ServiceName serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken));
}
