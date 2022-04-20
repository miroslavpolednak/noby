using CIS.InternalServices.ServiceDiscovery.Api.Repositories;

namespace CIS.InternalServices.ServiceDiscovery.Api.Handlers;

internal class GetServiceQueryHandler 
    : IRequestHandler<Dto.GetServiceRequest, Contracts.GetServiceResponse>
{
    public async Task<Contracts.GetServiceResponse> Handle(Dto.GetServiceRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(GetServiceQueryHandler));

        // query from cache
        var foundServices = await _cache.GetServices(request.Environment, cancellation);
        var service = foundServices.FirstOrDefault(t => t.ServiceType == request.ServiceType && t.ServiceName == request.ServiceName)
            ?? throw new CIS.Core.Exceptions.CisNotFoundException(0, nameof(Contracts.DiscoverableService));

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

    private readonly ILogger<GetServiceQueryHandler> _logger;
    private readonly ServicesMemoryCache _cache;

    public GetServiceQueryHandler(
        ILogger<GetServiceQueryHandler> logger,
        ServicesMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
    }
}
