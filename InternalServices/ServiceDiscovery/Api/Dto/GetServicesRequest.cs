using CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices.ServiceDiscovery.Api.Dto;

internal record GetServicesRequest(
    Core.Types.ApplicationEnvironmentName Environment,
    ServiceTypes ServiceType
)
    : IRequest<GetServicesResponse>
{ }
