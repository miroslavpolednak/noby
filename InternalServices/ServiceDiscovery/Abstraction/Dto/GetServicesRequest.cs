using MediatR;
using System.Collections.Generic;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction.Dto
{
    internal record GetServicesRequest(Core.Types.ApplicationEnvironmentName EnvironmentName)
        : IRequest<List<Contracts.DiscoverableService>>
    { }
}
