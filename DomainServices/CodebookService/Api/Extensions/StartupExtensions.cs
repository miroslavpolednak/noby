using CIS.Infrastructure.StartupExtensions;
using ProtoBuf.Grpc.Server;
using System.IO.Compression;
using CIS.Infrastructure.Caching;
using System.Reflection;

namespace DomainServices.CodebookService.Api;

internal static class StartupExtensions
{
    public static IServiceCollection AddCodebookService(this IServiceCollection services, AppConfiguration appConfiguration)
    {
        // add grpc
        services.AddCodeFirstGrpc(config => {
            config.ResponseCompressionLevel = CompressionLevel.Optimal;
            config.Interceptors.Add<CIS.Infrastructure.gRPC.SimpleServerExceptionInterceptor>();
        });

        // add current user context
        services.AddCisCurrentUser();

        // add distributed cache
        switch (appConfiguration.CacheType)
        {
            case CacheTypes.InMemory:
                services.AddInMemoryGlobalCache("CodebookCache");
                break;

            case CacheTypes.Redis:
                if (string.IsNullOrEmpty(appConfiguration.CacheConnectionString))
                    throw new ArgumentNullException("CacheConnectionString", "Redis connection string for Codebook Service Global Cache must be defined");

                services.AddRedisGlobalCache(appConfiguration.CacheConnectionString);
                break;
        }

        return services;
    }

    public static void AddCodebookServiceEndpointsStartup(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (Type type in assembly
              .GetTypes()
              .Where(mytype => mytype.GetInterfaces().Contains(typeof(Endpoints.ICodebookServiceEndpointStartup))))
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                ((Endpoints.ICodebookServiceEndpointStartup)assembly.CreateInstance(type.FullName ?? "")).Register(builder);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
        }
    }
}
