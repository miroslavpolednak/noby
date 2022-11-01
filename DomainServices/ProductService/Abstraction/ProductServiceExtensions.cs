using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.ProductService.Clients;

public static class ProductServiceExtensions
{
    public static IServiceCollection AddProductService(this IServiceCollection services)
        => services.TryAddGrpcClient<Contracts.v1.ProductService.ProductServiceClient>(a =>
            a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.v1.ProductService.ProductServiceClient>("DS:ProductService")
            .registerServices()
        );

    public static IServiceCollection AddProductService(this IServiceCollection services, string serviceUrl)
        => services.TryAddGrpcClient<Contracts.v1.ProductService.ProductServiceClient>(a =>
            a.AddGrpcServiceUriSettings<Contracts.v1.ProductService.ProductServiceClient>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.AddTransient<IProductServiceClient, Services.ProductService>();

        services.AddGrpcClientFromCisEnvironment<Contracts.v1.ProductService.ProductServiceClient>();

        return services;
    }
}
