using CIS.InternalServices.ServiceDiscovery.Api.Common;
using CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints.GetServices;

internal sealed class GetServicesHandler
    : IRequestHandler<GetServicesRequest, GetServicesResponse>
{
    public async Task<GetServicesResponse> Handle(GetServicesRequest request, CancellationToken cancellation)
    {
        // query from cache
        var foundServices = await _cache.GetServices(new(request.Environment), cancellation);

        var result = new GetServicesResponse { EnvironmentName = request.Environment };
        if (request.ServiceType != ServiceTypes.Unknown)
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
