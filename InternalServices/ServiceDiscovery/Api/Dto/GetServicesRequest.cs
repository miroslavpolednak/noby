using CIS.InternalServices.ServiceDiscovery.Contracts;
using MediatR;

namespace CIS.InternalServices.ServiceDiscovery.Dto;

internal record GetServicesRequest(
    Core.Types.ApplicationEnvironmentName Environment,
    ServiceTypes ServiceType
)
    : IRequest<GetServicesResponse>
{ }
