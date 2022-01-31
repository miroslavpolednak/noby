using CIS.Core.Types;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace CIS.InternalServices.ServiceDiscovery.Api.Handlers;

internal abstract class BaseGetQueryHandler
{
    protected readonly ILogger _logger;
    protected readonly ServiceDiscoveryRepository _repository;
    protected readonly Infrastructure.Caching.IGlobalCache _cache;

    public BaseGetQueryHandler(ILogger logger, ServiceDiscoveryRepository repository, Infrastructure.Caching.IGlobalCache cache)
    {
        _cache = cache;
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<Dto.ServiceModel>> LoadFromDatabase(ApplicationEnvironmentName environment)
    {
        // if cache fails, load from db
        var model = await _repository.GetList(environment);
        if (!model.Any())
        {
            _logger.NoServicesForEnvironment(environment);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"No services exists for environment '{environment}'", 102);
        }

        _logger.FoundServices(model.Count, environment);

        return model;
    }

    public void FillCache(List<Dto.ServiceModel> model, ApplicationEnvironmentName environment)
    {
        // save to cache
        var groups = model.GroupBy(t => t.ServiceType);
        foreach (var group in groups)
        {
            foreach (var item in group)
                _cache.Set(
                    environment,
                    new Infrastructure.Caching.HashItem(new ServiceNameCacheKey(item.ServiceType, item.ServiceName), item.ServiceUrl)
                );
        }
    }

    public List<Contracts.DiscoverableService>? GetFromCache(ApplicationEnvironmentName environment)
    {
        var set = _cache.GetHashset(environment);
        if (set == null) return null;

        return set.Select(t =>
            {
                var key = ServiceNameCacheKey.Deconstruct(t.Name);
                return new Contracts.DiscoverableService
                {
                    ServiceName = key.ServiceName,
                    ServiceType = key.ServiceType,
                    ServiceUrl = t.Value
                };
            })
            .ToList();
    }
}
