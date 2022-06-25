using CIS.InternalServices.ServiceDiscovery.Api.Repositories;

namespace CIS.InternalServices.ServiceDiscovery.Api.Handlers;

internal sealed class ClearCacheHandler
    : IRequestHandler<Dto.ClearCacheRequest, Google.Protobuf.WellKnownTypes.Empty>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.ClearCacheRequest request, CancellationToken cancellationToken)
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
