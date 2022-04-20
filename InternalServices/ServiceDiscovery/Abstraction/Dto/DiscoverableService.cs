using CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

public sealed record DiscoverableService(string ServiceName, string ServiceUrl, ServiceTypes ServiceType)
{
}
