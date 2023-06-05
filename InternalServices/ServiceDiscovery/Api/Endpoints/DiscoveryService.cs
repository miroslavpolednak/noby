using Grpc.Core;
using CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints;

internal sealed class DiscoveryService 
    : Contracts.v1.DiscoveryService.DiscoveryServiceBase
{
    /// <summary>
    /// Seznam vsech sluzeb registrovanych pro dane prostredi
    /// </summary>
    /// <exception cref="Core.Exceptions.CisInvalidEnvironmentNameException"></exception>
    /// <exception cref="RpcException">102</exception>
    public override async Task<GetServicesResponse> GetServices(GetServicesRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    /// <summary>
    /// Nastaveni konkretni sluzby pro dane prostredi
    /// </summary>
    /// <exception cref="Core.Exceptions.CisInvalidEnvironmentNameException"></exception>
    /// <exception cref="RpcException">101, 102, 103</exception>
    public override async Task<GetServiceResponse> GetService(GetServiceRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;
    public DiscoveryService(IMediator mediator)
        => _mediator = mediator;
}
