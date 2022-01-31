using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace CIS.InternalServices.ServiceDiscovery.Api.Handlers;

internal class GetServiceQueryHandler 
    : BaseGetQueryHandler, IRequestHandler<Dto.GetServiceRequest, Contracts.GetServiceResponse>
{
    public async Task<Contracts.GetServiceResponse> Handle(Dto.GetServiceRequest request, CancellationToken cancellation)
    {
        // query from cache
        if (!_cache.Exists(request.Environment))
            FillCache(await LoadFromDatabase(request.Environment), request.Environment);

        if (!_cache.TryGetHashset(request.Environment, new ServiceNameCacheKey(request.ServiceType, request.ServiceName), out string? url))
        {
            _logger.ServiceNotFound(request.ServiceName, request.ServiceType, request.Environment);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Service '{request.ServiceName}' not found for environment '{request.Environment}'", 103);
        }
        
        _logger.ServiceFoundInCache(request.ServiceName, request.ServiceType, request.Environment);

        return new Contracts.GetServiceResponse
        {
            EnvironmentName = request.Environment,
            Service = new Contracts.DiscoverableService
            {
                ServiceType = request.ServiceType,
                ServiceName = request.ServiceName,
                ServiceUrl = url
            }
        };
    }

    public GetServiceQueryHandler(
        ILogger<GetServicesQueryHandler> logger,
        ServiceDiscoveryRepository repository,
        Infrastructure.Caching.IGlobalCache cache)
        : base(logger, repository, cache) 
    { }
}
