using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace CIS.InternalServices.ServiceDiscovery.Api.Handlers;

internal class GetServicesQueryHandler 
    : BaseGetQueryHandler, IRequestHandler<Dto.GetServicesRequest, Contracts.GetServicesResponse>
{
    public async Task<Contracts.GetServicesResponse> Handle(Dto.GetServicesRequest request, CancellationToken cancellation)
    {
        // query from cache
        if (!_cache.Exists(request.Environment))
        {
            FillCache(await LoadFromDatabase(request.Environment), request.Environment);
        }

        // vytahnout sluzby z cache
        var foundServices = GetFromCache(request.Environment);
        if (foundServices == null || !foundServices.Any())
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Services not found for environment '{request.Environment}'", 103);

        _logger.FoundServices(foundServices.Count, request.Environment);

        var result = new Contracts.GetServicesResponse { EnvironmentName = request.Environment };

        if (request.ServiceType != Contracts.ServiceTypes.Unknown)
            result.Services.AddRange(foundServices.Where(t => t.ServiceType == request.ServiceType));
        else
            result.Services.AddRange(foundServices);

        return result;
    }

    public GetServicesQueryHandler(
        ILogger<GetServicesQueryHandler> logger, 
        ServiceDiscoveryRepository repository, 
        Infrastructure.Caching.IGlobalCache cache)
        : base(logger, repository, cache) 
    { }
}
