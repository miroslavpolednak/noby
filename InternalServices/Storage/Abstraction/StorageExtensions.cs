using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.InternalServices.Storage.Abstraction;

public static class StorageExtensions
{
    /// <summary>
    /// Override for integration testing
    /// </summary>
    internal static IServiceCollection AddStorageTest(this IServiceCollection services, Action<IServiceProvider, Grpc.Net.ClientFactory.GrpcClientFactoryOptions> customConfiguration)
    {
        services
            .AddGrpcClient<Contracts.v1.Blob.BlobClient>(customConfiguration);

        services
            .AddGrpcClient<Contracts.v1.BlobTemp.BlobTempClient>(customConfiguration);

        return services.registerServices();
    }

    public static IServiceCollection AddStorage(this IServiceCollection services, string storageServiceUrl)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.Blob.BlobClient>(storageServiceUrl)
            .AddGrpcServiceUriSettings<Contracts.v1.BlobTemp.BlobTempClient>(storageServiceUrl)
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddStorage(this IServiceCollection services)
        => services
            .AddCisServiceDiscovery()
            .registerUriSettings()
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.Blob.BlobClient>)))
        {
            services.TryAddSingleton(provider =>
            {
                string url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new(Core.Types.ServiceName.WellKnownServices.Storage), ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl ?? throw new ArgumentNullException("StorageServiceUrl");
                return new GrpcServiceUriSettings<Contracts.v1.Blob.BlobClient>(url);
            });

            services.TryAddSingleton(provider =>
            {
                string url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new(Core.Types.ServiceName.WellKnownServices.Storage), ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl ?? throw new ArgumentNullException("StorageServiceUrl");
                return new GrpcServiceUriSettings<Contracts.v1.BlobTemp.BlobTempClient>(url);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // mediatr
        services.AddMediatR(typeof(StorageExtensions).Assembly);

        // register storage services
        services.TryAddTransient<IBlobServiceAbstraction, BlobStorage.BlobService>();
        services.TryAddTransient<IBlobTempServiceAbstraction, BlobStorageTemp.BlobTempService>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.Blob.BlobClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.Blob.BlobClient>()
                .AddInterceptor<ExceptionInterceptor>();

            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.BlobTemp.BlobTempClient>()
                .AddInterceptor<ExceptionInterceptor>();
        }
        return services;
    }
}
