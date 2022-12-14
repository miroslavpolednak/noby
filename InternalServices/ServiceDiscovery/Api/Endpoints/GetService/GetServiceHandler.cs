using CIS.InternalServices.ServiceDiscovery.Api.Common;
using CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints.GetService;

internal sealed class GetServiceHandler
    : IRequestHandler<GetServiceRequest, GetServiceResponse>
{
    public async Task<GetServiceResponse> Handle(GetServiceRequest request, CancellationToken cancellation)
    {
        // query from cache
        var foundServices = await _cache.GetServices(new(request.Environment), cancellation);
        var service = foundServices.FirstOrDefault(t => t.ServiceType == request.ServiceType && t.ServiceName == request.ServiceName)
            ?? throw new CisNotFoundException(0, nameof(DiscoverableService));

        return new GetServiceResponse
        {
            EnvironmentName = request.Environment,
            Service = new DiscoverableService
            {
                ServiceType = request.ServiceType,
                ServiceName = request.ServiceName,
                ServiceUrl = service.ServiceUrl
            }
        };
    }

    private readonly ServicesMemoryCache _cache;

    public GetServiceHandler(ServicesMemoryCache cache)
    {
        _cache = cache;
    }
}
