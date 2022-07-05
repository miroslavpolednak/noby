using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.ProductService.Abstraction;

public static class ProductServiceExtensions
{
    public static IServiceCollection AddProductService(this IServiceCollection services)
        => services
            .AddCisServiceDiscovery()
            .registerUriSettings()
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddProductService(this IServiceCollection services, string serviceUrl)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.ProductService.ProductServiceClient>(serviceUrl)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.ProductService.ProductServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetServiceUrlSynchronously(new("DS:ProductService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc);
                return new GrpcServiceUriSettings<Contracts.v1.ProductService.ProductServiceClient>(url ?? throw new ArgumentNullException("url", "ProductService URL can not be determined"));
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.TryAddTransient<IProductServiceAbstraction, Services.ProductService>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.ProductService.ProductServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.ProductService.ProductServiceClient>();
        }
        return services;
    }
}
