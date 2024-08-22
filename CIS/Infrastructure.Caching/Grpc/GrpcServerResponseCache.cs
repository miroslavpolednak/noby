using CIS.Core.Configuration;
using Microsoft.Extensions.Caching.Distributed;

namespace CIS.Infrastructure.Caching.Grpc;

internal sealed class GrpcServerResponseCache(
    ICisEnvironmentConfiguration _environmentConfiguration,
    IDistributedCache _distributedCache)
    : IGrpcServerResponseCache
{
    public async Task InvalidateEntry(string methodName, object key)
    {
        await _distributedCache.RemoveAsync(GrpcClientResponseCacheHelpers.CreateCacheKey(_environmentConfiguration.DefaultApplicationKey![(_environmentConfiguration.DefaultApplicationKey!.IndexOf(':') + 1)..], methodName, key));
    }

    public async Task InvalidateEntry(string serviceName, string methodName, object key)
    {
        var index = serviceName.IndexOf(':'); // nedava se sem prefix DS:

        await _distributedCache.RemoveAsync(GrpcClientResponseCacheHelpers.CreateCacheKey(index > 0 ? serviceName[(index + 1)..] : serviceName, methodName, key));
    }
}
