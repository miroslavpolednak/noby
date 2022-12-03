using CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints.GetServices;

internal record GetServicesRequest(
    Core.Types.ApplicationEnvironmentName Environment,
    ServiceTypes ServiceType
)
    : IRequest<GetServicesResponse>
{ }
