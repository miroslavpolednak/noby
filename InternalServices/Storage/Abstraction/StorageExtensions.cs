using CIS.InternalServices.ServiceDiscovery.Abstraction;
using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.DomainServicesSecurity.Abstraction;

namespace CIS.InternalServices.Storage.Abstraction;

public static class StorageExtensions
{
    /// <summary>
    /// Override for integration testing
    /// </summary>
    internal static IServiceCollection AddStorageTest(this IServiceCollection services, Action<IServiceProvider, Grpc.Net.ClientFactory.GrpcClientFactoryOptions> customConfiguration)
    {
        services
            .AddGrpcClient<Contracts.v1.Blob.BlobClient>(customConfiguration)
            .AddInterceptor<ExceptionInterceptor>()
            .AddInterceptor<AuthenticationInterceptor>();

        services
            .AddGrpcClient<Contracts.v1.BlobTemp.BlobTempClient>(customConfiguration)
            .AddInterceptor<ExceptionInterceptor>()
            .AddInterceptor<AuthenticationInterceptor>();

        return services.registerServices();
    }

    public static IServiceCollection AddStorage(this IServiceCollection services, string storageServiceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.Blob.BlobClient>(storageServiceUrl, isInvalidCertificateAllowed)
            .AddGrpcServiceUriSettings<Contracts.v1.BlobTemp.BlobTempClient>(storageServiceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddStorage(this IServiceCollection services, bool isInvalidCertificateAllowed)
        => services
            .AddCisServiceDiscovery(isInvalidCertificateAllowed)
            .registerUriSettings(isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
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
                return new GrpcServiceUriSettings<Contracts.v1.Blob.BlobClient>(url, isInvalidCertificateAllowed);
            });

            services.TryAddSingleton(provider =>
            {
                string url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new(Core.Types.ServiceName.WellKnownServices.Storage), ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl ?? throw new ArgumentNullException("StorageServiceUrl");
                return new GrpcServiceUriSettings<Contracts.v1.BlobTemp.BlobTempClient>(url, isInvalidCertificateAllowed);
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

        // exception handling
        services.TryAddSingleton<ExceptionInterceptor>();
        services.TryAddSingleton<AuthenticationInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.Blob.BlobClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.Blob.BlobClient>()
                .AddInterceptor<ExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();

            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.BlobTemp.BlobTempClient>()
                .AddInterceptor<ExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();
        }
        return services;
    }
}
