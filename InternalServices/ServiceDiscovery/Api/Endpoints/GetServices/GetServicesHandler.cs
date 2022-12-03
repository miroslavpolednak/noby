using CIS.InternalServices.ServiceDiscovery.Api.Common;

namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints.GetServices;

internal sealed class GetServicesHandler
    : IRequestHandler<GetServicesRequest, Contracts.GetServicesResponse>
{
    public async Task<Contracts.GetServicesResponse> Handle(GetServicesRequest request, CancellationToken cancellation)
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

    private readonly ServicesMemoryCache _cache;

    public GetServicesHandler(ServicesMemoryCache cache)
    {
        _cache = cache;
    }
}
