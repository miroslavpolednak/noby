using CIS.Core.Types;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

public interface IDiscoveryServiceAbstraction
{
    Task<List<Contracts.DiscoverableService>> GetServices();

    Task<List<Contracts.DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName);

    Task<Contracts.DiscoverableService> GetService(ServiceName serviceName, Contracts.ServiceTypes serviceType);

    Task<Contracts.DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ServiceName serviceName, Contracts.ServiceTypes serviceType);
}
