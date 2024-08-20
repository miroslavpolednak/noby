using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.Caching.Grpc;

public static class GrpcClientResponseCachingStartupExtensions
{
    public static void AddGrpcClientResponseCaching<TClient>(this IServiceCollection services, in string serviceName)
        where TClient : class
    {
        string name = serviceName[(serviceName.IndexOf(':') + 1)..];

        services.TryAddScoped<IGrpcClientResponseCache<TClient>>(provider =>
        {
            var cache = provider.GetService<IDistributedCache>();
            return new GrpcClientResponseCache<TClient>(name, cache);
        });
    }
}