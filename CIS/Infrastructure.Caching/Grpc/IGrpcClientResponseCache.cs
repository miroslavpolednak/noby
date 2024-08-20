using System.Runtime.CompilerServices;

namespace CIS.Infrastructure.Caching.Grpc;

public interface IGrpcClientResponseCache<TClient>
    where TClient : class
{
    Task<TResponse> GetResponse<TResponse>(
        object key,
        Func<Task<TResponse>> getObject,
        CancellationToken cancellationToken = default,
        [CallerMemberName]  string memberName = "")
        where TResponse : class;
}