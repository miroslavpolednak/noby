using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.CompilerServices;

namespace CIS.Infrastructure.Caching.Grpc;

public interface IGrpcClientResponseCache<TClient>
    where TClient : class
{
    Task<TResponse> GetLocalOrDistributed<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        DistributedCacheEntryOptions distributedCacheOptions,
        CancellationToken cancellationToken = default,
        [CallerMemberName]  string memberName = "")
        where TResponse : class;

    Task<TResponse> GetDistributedOnly<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        DistributedCacheEntryOptions distributedCacheOptions,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string memberName = "")
        where TResponse : class;

    Task<TResponse> GetLocalOnly<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string memberName = "")
        where TResponse : class;

    Task<TResponse> GetLocalOrDistributed<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        DistributedCacheEntryOptions distributedCacheOptions,
        Core.Configuration.ICisDistributedCacheConfiguration.SerializationTypes serializationType,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string memberName = "")
        where TResponse : class;

    Task<TResponse> GetDistributedOnly<TResponse>(
        object key,
        Func<CancellationToken, Task<TResponse>> getObject,
        DistributedCacheEntryOptions distributedCacheOptions,
        Core.Configuration.ICisDistributedCacheConfiguration.SerializationTypes serializationType,
        CancellationToken cancellationToken = default,
        [CallerMemberName] string memberName = "")
        where TResponse : class;

    void InvalidateLocalCache();
}