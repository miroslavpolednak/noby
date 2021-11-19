using CIS.Core.Types;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction.Dto
{
    internal record GetServiceRequest(ApplicationEnvironmentName EnvironmentName, ServiceName ServiceName, Contracts.ServiceTypes ServiceType)
        : IRequest<Contracts.DiscoverableService>
    { }
}
