using CIS.Core.Types;
using System.Collections.Immutable;

namespace CIS.InternalServices.ServiceDiscovery.Clients;

public interface IDiscoveryServiceAbstraction
{
    Task<ImmutableList<DiscoverableService>> GetServices(CancellationToken cancellationToken = default(CancellationToken));

    Task<ImmutableList<DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName, CancellationToken cancellationToken = default(CancellationToken));

    Task<DiscoverableService> GetService(ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken));

    Task<DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ApplicationKey serviceName, Contracts.ServiceTypes serviceType, CancellationToken cancellationToken = default(CancellationToken));

    string GetServiceUrlSynchronously(ApplicationKey serviceName, Contracts.ServiceTypes serviceType);
}
