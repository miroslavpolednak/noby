using CIS.InternalServices.ServiceDiscovery.Api.Common;

namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints.ClearCache;

internal sealed class ClearCacheHandler
    : IRequestHandler<ClearCacheRequest, Google.Protobuf.WellKnownTypes.Empty>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(ClearCacheRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        ServicesMemoryCache.Clear();

        _logger.CacheCleared();

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly ILogger<ClearCacheHandler> _logger;

    public ClearCacheHandler(ILogger<ClearCacheHandler> logger)
    {
        _logger = logger;
    }
}
