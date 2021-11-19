using CIS.Core.Types;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

internal partial class DiscoveryService : IDiscoveryServiceAbstraction
{
    public async Task<List<Contracts.DiscoverableService>> GetServices()
        => await GetServices(getEnvName());
    
    public async Task<List<Contracts.DiscoverableService>> GetServices(ApplicationEnvironmentName environmentName)
        => await _mediator.Send(new Dto.GetServicesRequest(environmentName));
    
    public async Task<Contracts.DiscoverableService> GetService(ServiceName serviceName, Contracts.ServiceTypes serviceType)
        => await GetService(getEnvName(), serviceName, serviceType);

    public async Task<Contracts.DiscoverableService> GetService(ApplicationEnvironmentName environmentName, ServiceName serviceName, Contracts.ServiceTypes serviceType)
        => await _mediator.Send(new Dto.GetServiceRequest(environmentName, serviceName, serviceType));
}

internal partial class DiscoveryService
{
    private readonly IMediator _mediator;
    private readonly EnvironmentNameProvider _envName;

    public DiscoveryService(
        IMediator mediator,
        EnvironmentNameProvider envName)
    {
        _mediator = mediator;
        _envName = envName;
    }

    private ApplicationEnvironmentName getEnvName()
        => _envName.Name ?? throw new InvalidOperationException("EnvironmentName");
}
