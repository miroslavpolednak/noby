using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace CIS.Infrastructure.Caching.Grpc;

internal sealed class GrpcClientResponseCache<TClient>
    : IGrpcClientResponseCache<TClient>
    where TClient : class
{
    /// <summary>
    /// nazev sluzby, jejiz klient se ma kesovat
    /// </summary>
    private readonly string _serviceName;

    /// <summary>
    /// muze byt null kdyz aplikace nema distribuovanou cache
    /// </summary>
    private readonly IDistributedCache? _distributedCache;

    private readonly ILogger<GrpcClientResponseCache<TClient>> _logger;

    public GrpcClientResponseCache(string serviceName, IDistributedCache? distributedCache, ILogger<GrpcClientResponseCache<TClient>> logger)
    {
        _logger = logger;
        _distributedCache = distributedCache;
        _serviceName = serviceName;
    }

    public async Task<TResponse> GetLocalOrDistributed<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        DistributedCacheEntryOptions distributedCacheOptions,
        CancellationToken cancellationToken,
        [CallerMemberName] string memberName = "")
        where TResponse : class
    {
        if (_localCacheValues.TryGetValue((memberName, key), out var cachedValue))
        {
            _logger.UsingCacheInsteadOfRpc(LoggerExtensions.CacheTypes.Local, memberName, _serviceName);
            return (TResponse)cachedValue;
        }
        else if (_distributedCache is not null) // mame zapnutou distr. cache, zkusime ziskat hodnotu z ni
        {
            var distCachedValue = await _distributedCache.GetOrAddObjectAsync<TResponse>(
                GrpcClientResponseCacheHelpers.CreateCacheKey(_serviceName, memberName, key),
                getObject,
                distributedCacheOptions,
                GrpcClientResponseCacheHelpers.DistributedCacheSerializationType,
                cancellationToken);

            if (distCachedValue is not null)
            {
                _logger.UsingCacheInsteadOfRpc(LoggerExtensions.CacheTypes.Distributed, memberName, _serviceName);
                _localCacheValues.Add((memberName, key), distCachedValue);
                return distCachedValue;
            }
        }

        var response = await getObject(cancellationToken);
        _localCacheValues.Add((memberName, key), response);
        return response;
    }

    public async Task<TResponse> GetDistributedOnly<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        DistributedCacheEntryOptions distributedCacheOptions,
        CancellationToken cancellationToken,
        [CallerMemberName] string memberName = "")
        where TResponse : class
    {
        if (_distributedCache is not null)
        {
            var distCachedValue = await _distributedCache.GetOrAddObjectAsync<TResponse>(
                GrpcClientResponseCacheHelpers.CreateCacheKey(_serviceName, memberName, key),
                getObject,
                distributedCacheOptions,
                GrpcClientResponseCacheHelpers.DistributedCacheSerializationType,
                cancellationToken);

            if (distCachedValue is not null)
            {
                _logger.UsingCacheInsteadOfRpc(LoggerExtensions.CacheTypes.Distributed, memberName, _serviceName);
                return distCachedValue;
            }
        }

        return await getObject(cancellationToken);
    }

    public async Task<TResponse> GetLocalOnly<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        CancellationToken cancellationToken,
        [CallerMemberName] string memberName = "")
        where TResponse : class
    {
        if (_localCacheValues.TryGetValue((memberName, key), out var cachedValue))
        {
            _logger.UsingCacheInsteadOfRpc(LoggerExtensions.CacheTypes.Local, memberName, _serviceName);
            return (TResponse)cachedValue;
        }
        else
        {
            var response = await getObject(cancellationToken);
            _localCacheValues.Add((memberName, key), response);
            return response;
        }
    }

    /// <summary>
    /// Lokalni scoped uloziste pro nakesovane responses
    /// </summary>
    private readonly Dictionary<(string MethodName, object Key), object> _localCacheValues = [];
}

