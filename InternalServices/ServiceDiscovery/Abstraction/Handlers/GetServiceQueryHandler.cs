using Microsoft.Extensions.Logging;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction.Handlers;

internal class GetServiceQueryHandler 
    : IRequestHandler<Dto.GetServiceRequest, Contracts.DiscoverableService>
{
    public async Task<Contracts.DiscoverableService> Handle(Dto.GetServiceRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Get {serviceName}:{serviceType} from {environment}", request.ServiceName, request.EnvironmentName.Name, request.ServiceType);

        string? cachedServiceUrl = null;
        var cacheKey = new ServiceNameCacheKey(request.ServiceType, request.ServiceName);
        bool foundInCache = _cache?.TryGetHashset(request.EnvironmentName, cacheKey, out cachedServiceUrl) ?? false;

        if (!foundInCache) // neni v kesi
        {
            _logger.LogDebug("Service {serviceName}:{serviceType} from {environment} not found in cache", request.ServiceName, request.EnvironmentName.Name, request.ServiceType);

            var model = new Contracts.GetServiceRequest
            {
                Environment = request.EnvironmentName,
                ServiceType = request.ServiceType,
                ServiceName = request.ServiceName
            };

            var result = await _userContext.AddUserContext(async () => (await _service.GetServiceAsync(model)));

            _logger.LogDebug("Service {serviceName}:{serviceType} from {environment} obtained", request.ServiceName, request.EnvironmentName.Name, request.ServiceType);

            _cache?.Set(request.EnvironmentName, new Infrastructure.Caching.HashItem(cacheKey, result.Service.ServiceUrl));
            return new Contracts.DiscoverableService { ServiceName = result.Service.ServiceName, ServiceUrl = result.Service.ServiceUrl };
        }
        else
        {
            _logger.LogDebug("Service {serviceName}:{serviceType} from {environment} FOUND in cache", request.ServiceName, request.EnvironmentName.Name, request.ServiceType);
            return new Contracts.DiscoverableService { ServiceName = request.ServiceName, ServiceType = request.ServiceType, ServiceUrl = cachedServiceUrl };
        }
    }

    private readonly Infrastructure.Caching.IGlobalCache<DiscoveryService> _cache;
    private readonly ILogger<GetServiceQueryHandler> _logger;
    private readonly Contracts.v1.DiscoveryService.DiscoveryServiceClient _service;
    private readonly Security.InternalServices.ICisUserContextHelpers _userContext;

    public GetServiceQueryHandler(
        Contracts.v1.DiscoveryService.DiscoveryServiceClient service, 
        ILogger<GetServiceQueryHandler> logger, 
        Infrastructure.Caching.IGlobalCache<DiscoveryService> cache, 
        Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _cache = cache;
        _logger = logger;
        _service = service;
    }
}
