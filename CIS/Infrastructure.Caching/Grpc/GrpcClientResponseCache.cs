using Microsoft.Extensions.Caching.Distributed;
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

    public GrpcClientResponseCache(string serviceName, IDistributedCache? distributedCache)
    {
        _distributedCache = distributedCache;
        _serviceName = serviceName;
    }

    public async Task<TResponse> GetResponse<TResponse>(
        object key,
        Func<Task<TResponse>> getObject,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string memberName = "")
        where TResponse : class
    {
        if (_localCacheValues.TryGetValue((memberName, key), out var cachedValue))
        {
            return (TResponse)cachedValue;
        }
        else if (_distributedCache is not null) // mame zapnutou distr. cache, zkusime ziskat hodnotu z ni
        {
            var distCachedValue = await _distributedCache.GetObjectAsync<TResponse>(getDistributedCacheKey(_serviceName, memberName, key), Core.Configuration.ICisDistributedCacheConfiguration.SerializationTypes.Json, cancellationToken);
            if (distCachedValue is not null)
            {
                _localCacheValues.Add((memberName, key), distCachedValue);
                return distCachedValue;
            }
        }

        var response = await getObject();
        _localCacheValues.Add((memberName, key), response);
        return response;
    }

    private readonly Dictionary<(string MethodName, object Key), object> _localCacheValues = [];

    private static string getDistributedCacheKey(in string serviceName, in string methodName, in object key)
    {
        return $"GDCC:{serviceName}-{methodName}-{key}";
    }
}

