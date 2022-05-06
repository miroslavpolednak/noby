using CIS.InternalServices.ServiceDiscovery.Api.Repositories;

namespace CIS.InternalServices.ServiceDiscovery.Api.Handlers;

internal class GetServicesQueryHandler 
    : IRequestHandler<Dto.GetServicesRequest, Contracts.GetServicesResponse>
{
    public async Task<Contracts.GetServicesResponse> Handle(Dto.GetServicesRequest request, CancellationToken cancellation)
    {
        // query from cache
        var foundServices = await _cache.GetServices(request.Environment, cancellation);

        var result = new Contracts.GetServicesResponse { EnvironmentName = request.Environment };
        if (request.ServiceType != Contracts.ServiceTypes.Unknown)
            result.Services.AddRange(foundServices.Where(t => t.ServiceType == request.ServiceType));
        else
            result.Services.AddRange(foundServices);

        return result;
    }

    private readonly ILogger<GetServicesQueryHandler> _logger;
    private readonly ServicesMemoryCache _cache;

    public GetServicesQueryHandler(
        ILogger<GetServicesQueryHandler> logger,
        ServicesMemoryCache cache)
    { 
        _logger = logger;
        _cache = cache;
    }
}
