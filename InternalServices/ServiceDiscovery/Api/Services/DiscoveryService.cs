using CIS.Core.Types;
using Grpc.Core;
using CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices.ServiceDiscovery.Api.Services;

internal class DiscoveryService : Contracts.v1.DiscoveryService.DiscoveryServiceBase
{
    private readonly IMediator _mediator;

    public DiscoveryService(IMediator mediator)
        => this._mediator = mediator;

    /// <summary>
    /// Seznam vsech sluzeb registrovanych pro dane prostredi
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisInvalidEnvironmentNameException"></exception>
    /// <exception cref="Grpc.Core.RpcException">102</exception>
    public override async Task<GetServicesResponse> GetServices(GetServicesRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetServicesRequest(new(request.Environment), request.ServiceType), context.CancellationToken);

    /// <summary>
    /// Nastaveni konkretni sluzby pro dane prostredi
    /// </summary>
    /// <exception cref="CIS.Core.Exceptions.CisInvalidEnvironmentNameException"></exception>
    /// <exception cref="Grpc.Core.RpcException">101, 102, 103</exception>
    public override async Task<GetServiceResponse> GetService(GetServiceRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetServiceRequest(new(request.Environment), new(request.ServiceName), request.ServiceType), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> ClearCache(ClearCacheRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.ClearCacheRequest(), context.CancellationToken);
}
