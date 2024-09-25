using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
        CancellationToken cancellationToken = default,
        [CallerMemberName] string memberName = "")
        where TResponse : class
        => await GetLocalOrDistributed(key, getObject, distributedCacheOptions, GrpcClientResponseCacheHelpers.DistributedCacheSerializationType, cancellationToken, memberName);

    public async Task<TResponse> GetLocalOrDistributed<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        DistributedCacheEntryOptions distributedCacheOptions,
        Core.Configuration.ICisDistributedCacheConfiguration.SerializationTypes serializationType,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string memberName = "")
        where TResponse : class
    {
        if (_localCacheValues.TryGetValue((memberName, key), out var cachedValue))
        {
            logPayload(cachedValue, key, LoggerExtensions.CacheTypes.Local, memberName);
            return (TResponse)cachedValue;
        }
        else if (_distributedCache is not null) // mame zapnutou distr. cache, zkusime ziskat hodnotu z ni
        {
            var distCachedValue = await _distributedCache.GetOrAddObjectAsync<TResponse>(
                GrpcClientResponseCacheHelpers.CreateCacheKey(_serviceName, memberName, key),
                getObject,
                distributedCacheOptions,
                serializationType,
                cancellationToken);

            if (distCachedValue is not null)
            {
                logPayload(distCachedValue, key, LoggerExtensions.CacheTypes.Distributed, memberName);
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
        CancellationToken cancellationToken = default,
        [CallerMemberName] string memberName = "")
        where TResponse : class
        => await GetDistributedOnly(key, getObject, distributedCacheOptions, GrpcClientResponseCacheHelpers.DistributedCacheSerializationType, cancellationToken, memberName);

    public async Task<TResponse> GetDistributedOnly<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        DistributedCacheEntryOptions distributedCacheOptions,
        Core.Configuration.ICisDistributedCacheConfiguration.SerializationTypes serializationType,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string memberName = "")
        where TResponse : class
    {
        if (_distributedCache is not null)
        {
            var distCachedValue = await _distributedCache.GetOrAddObjectAsync<TResponse>(
                GrpcClientResponseCacheHelpers.CreateCacheKey(_serviceName, memberName, key),
                getObject,
                distributedCacheOptions,
                serializationType,
                cancellationToken);

            if (distCachedValue is not null)
            {
                logPayload(distCachedValue, key, LoggerExtensions.CacheTypes.Distributed, memberName);
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
            logPayload(cachedValue, key, LoggerExtensions.CacheTypes.Local, memberName);
            return (TResponse)cachedValue;
        }
        else
        {
            var response = await getObject(cancellationToken);
            _localCacheValues.Add((memberName, key), response);
            return response;
        }
    }

    public void InvalidateLocalCache()
    {
        _localCacheValues.Clear();
    }

    private void logPayload(in object cachedValue, [NotNull] in object key, LoggerExtensions.CacheTypes cacheType, in string memberName)
    {
        // it is pointless to serialize payload when log level prohibits logging of this event
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", System.Text.Json.JsonSerializer.Serialize(cachedValue) }
            }))
            {
                _logger.UsingCacheInsteadOfRpc(cacheType, key.ToString()!, memberName, _serviceName);
            }
        }
    }

    /// <summary>
    /// Lokalni scoped uloziste pro nakesovane responses
    /// </summary>
    private readonly Dictionary<(string MethodName, object Key), object> _localCacheValues = [];
}

