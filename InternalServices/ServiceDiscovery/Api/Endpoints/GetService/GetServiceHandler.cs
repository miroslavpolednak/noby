using CIS.InternalServices.ServiceDiscovery.Api.Common;

namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints.GetService;

internal sealed class GetServiceHandler
    : IRequestHandler<GetServiceRequest, Contracts.GetServiceResponse>
{
    public async Task<Contracts.GetServiceResponse> Handle(GetServiceRequest request, CancellationToken cancellation)
    {
        // query from cache
        var foundServices = await _cache.GetServices(request.Environment, cancellation);
        var service = foundServices.FirstOrDefault(t => t.ServiceType == request.ServiceType && t.ServiceName == request.ServiceName)
            ?? throw new Core.Exceptions.CisNotFoundException(0, nameof(Contracts.DiscoverableService));

        return new Contracts.GetServiceResponse
        {
            EnvironmentName = request.Environment,
            Service = new Contracts.DiscoverableService
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
